﻿using Newtonsoft.Json;
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
                _context.Entry(inputEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;               
            }
            else
            {
                inputEntity = new InputOutput
                {
                    Name = viewModel.Name,
                    TSSystemId = viewModel.SystemId,
                    Type = IOType.Input
                };
                _context.InputsOutputs.Add(inputEntity);
                _context.SaveChanges();
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
    }
}