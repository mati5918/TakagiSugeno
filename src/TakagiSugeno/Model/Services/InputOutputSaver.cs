using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Services
{
    public class InputOutputSaver
    {
        private TakagiSugenoDbContext _context;
        public InputOutputSaver(TakagiSugenoDbContext context)
        {
            _context = context;
        }

        public void Save(InputVM viewModel)
        {
            InputOutput inputEntity = _context.InputsOutputs.FirstOrDefault(i => i.InputOutputId == viewModel.InputId);
            if (inputEntity != null)
            {
                inputEntity.Name = viewModel.Name;
                inputEntity.RangeStart = viewModel.RangeStart;
                inputEntity.RangeEnd = viewModel.RangeEnd;
                _context.Entry(inputEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;               
            }
            else
            {
                inputEntity = new InputOutput
                {
                    Name = viewModel.Name,
                    TSSystemId = viewModel.SystemId,
                    Type = IOType.Input,
                    RangeEnd = viewModel.RangeEnd,
                    RangeStart = viewModel.RangeStart
                };
                _context.InputsOutputs.Add(inputEntity);
                _context.SaveChanges();
                AddInputOutputToRules(inputEntity);
                viewModel.InputId = inputEntity.InputOutputId;
            }
            SaveVariables(viewModel.InputId, viewModel.Variables);
            _context.SaveChanges();
        }

        public void Save(OutputVM viewModel)
        {
            InputOutput outputEntity = _context.InputsOutputs.FirstOrDefault(i => i.InputOutputId == viewModel.OutputId);
            if (outputEntity != null)
            {
                outputEntity.Name = viewModel.Name;
                _context.Entry(outputEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                outputEntity = new InputOutput
                {
                    Name = viewModel.Name,
                    TSSystemId = viewModel.SystemId,
                    Type = IOType.Output
                };
                _context.InputsOutputs.Add(outputEntity);
                _context.SaveChanges();
                AddInputOutputToRules(outputEntity);
                viewModel.OutputId = outputEntity.InputOutputId;
            }
            SaveVariables(viewModel.OutputId, viewModel.Variables);
            _context.SaveChanges();
        }

        private void SaveVariables(int id, List<VariableVM> variables)
        {
            IEnumerable<Variable> variableEntities = _context.Variables.Where(v => v.InputOutputId == id);
            IEnumerable<VariableVM> existedVariables = variables.Where(v => v.VariableId >= 0);
            IEnumerable<VariableVM> newVariables = variables.Where(v => v.VariableId < 0);
            foreach (var variable in variableEntities)
            {
                VariableVM itemToUpdate = existedVariables.FirstOrDefault(v => v.VariableId == variable.VariableId);
                if (itemToUpdate != null)
                {
                    variable.Type = itemToUpdate.Type;
                    variable.Name = itemToUpdate.Name;
                    variable.Data = JsonConvert.SerializeObject(itemToUpdate.FunctionData);
                    _context.Entry(variable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else
                {
                    RemoveVariableFromRules(variable);
                    _context.Variables.Remove(variable);
                }
            }
            foreach (var variable in newVariables)
            {
                Variable newVariable = new Variable
                {
                    Name = variable.Name,
                    Type = variable.Type,
                    Data = JsonConvert.SerializeObject(variable.FunctionData),
                    InputOutputId = id
                };
                _context.Variables.Add(newVariable);
            }
        }

        public void ModifyOutputsVariables(int systemId, string newName, string oldName, ModifyVariableAction action)
        {
            var variables = _context.Variables.Where(v => v.InputOutput.TSSystemId == systemId && v.Type == VariableType.OutputFunction);
            foreach (var v in variables)
            {
                Dictionary<string, double> oldData = JsonConvert.DeserializeObject<Dictionary<string, double>>(v.Data);
                Dictionary<string, double> newData = new Dictionary<string, double>();
                switch (action)
                {
                    case ModifyVariableAction.Change:
                        foreach (var item in oldData)
                        {
                            newData.Add(item.Key == oldName ? newName : item.Key, item.Value);
                        }
                        v.Data = JsonConvert.SerializeObject(newData);
                        break;
                    case ModifyVariableAction.Delete:
                        oldData.Remove(oldName);
                        v.Data = JsonConvert.SerializeObject(oldData);
                        break;
                    case ModifyVariableAction.Add:
                        oldData.Add(newName, 0);
                        v.Data = JsonConvert.SerializeObject(oldData);
                        break;
                }
                _context.Entry(v).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            _context.SaveChanges();
        }

        private void RemoveVariableFromRules(Variable variable)
        {
            var elements = _context.RuleElements.Where(e => e.VariableId == variable.VariableId);
            foreach(var elem in elements)
            {
                elem.VariableId = null;
                elem.IsNegation = false;
                _context.Entry(elem).State = EntityState.Modified;
            }
        }

        private void AddInputOutputToRules(InputOutput io)
        {
            var rules = _context.Rules.Include(r => r.RuleElements).Where(r => r.TSSystemId == io.TSSystemId);
            foreach(var rule in rules)
            {
                RuleElement elem = new RuleElement
                {
                    InputOutputId = io.InputOutputId,
                    IsNegation = false,
                    NextOpartion = RuleNextOperation.None,
                    RuleId = rule.RuleId,
                    VariableId = null,
                    Type = io.Type == IOType.Input ? RuleElementType.InputPart : RuleElementType.OutputPart
                };
                if (io.Type == IOType.Input)
                {
                    var last = rule.RuleElements.FirstOrDefault(e => e.Type == RuleElementType.InputPart && e.NextOpartion == RuleNextOperation.None);
                    if (last != null)
                    {
                        last.NextOpartion = RuleNextOperation.And;
                        _context.Entry(last).State = EntityState.Modified;
                    }
                }
                _context.RuleElements.Add(elem);
                
            }
            _context.SaveChanges();
        }

        public void RemoveRuleElements(InputOutput io)
        {
            var rules = _context.Rules.Include(r => r.RuleElements).Where(r => r.TSSystemId == io.TSSystemId);
            foreach(var rule in rules)
            {
                var elem = rule.RuleElements.FirstOrDefault(e => e.InputOutputId == io.InputOutputId);
                _context.RuleElements.Remove(elem);
                if(elem.Type == RuleElementType.InputPart && elem.NextOpartion == RuleNextOperation.None)
                {
                    var lastElem = rule.RuleElements.LastOrDefault(e => e.Type == RuleElementType.InputPart && e.NextOpartion != RuleNextOperation.None);
                    if (lastElem != null)
                    {
                        lastElem.NextOpartion = RuleNextOperation.None;
                        _context.Entry(lastElem).State = EntityState.Modified;
                    }
                }
            }
            _context.SaveChanges();
        }
    }
    public enum ModifyVariableAction
    {
        Change,
        Add,
        Delete
    }
}
