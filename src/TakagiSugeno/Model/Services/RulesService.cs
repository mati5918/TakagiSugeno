using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Services
{
    public class RulesService
    {
        private IRepository<InputOutput> _ioRepository;
        private IRepository<Rule> _ruleRepository;
        private RulesSaver _saver;

        public RulesService(IRepository<Rule> ruleRepository, IRepository<InputOutput> ioRepository, RulesSaver saver)
        {
            _ruleRepository = ruleRepository;
            _ioRepository = ioRepository;
            _saver = saver;
        }

        public RuleGeneralVM GetSystemRules(int systemId)
        {
            RuleGeneralVM res = new RuleGeneralVM();
            res.Rules = _ruleRepository.GetBySystemId(systemId)
                .Select(r => new RuleVM
                {
                    RuleId = r.RuleId,
                    SystemId = r.TSSystemId,
                    RuleElements = r.RuleElements.OrderBy(e => e.Type)
                .Select(elem => new RuleElementVM
                {
                    ElementId = elem.RuleElementId,
                    Type = elem.Type,
                    InputOutputId = elem.InputOutputId,
                    VariableId = elem.VariableId.HasValue ? elem.VariableId.Value : -1,
                    InputOutputName = elem.InputOutput.Name,
                    VariableName = elem.Variable?.Name,
                    NextOperation = elem.NextOpartion,
                    IsNegation = elem.IsNegation
                }).ToList()
                }).ToList();
            res.VariablesLists = CreateVariablesLists(systemId);
            res.ChartsData = CreateChartsData(systemId);
            return res;
        }

        public List<string> GetSystemInputsOutputsNames(int systemId)
        {
            return _ioRepository.GetBySystemId(systemId).OrderBy(io => io.Type).Select(io => io.Name).ToList();
        }

        public Dictionary<int, IEnumerable<SelectListItem>> CreateVariablesLists(int systemId)
        {
            Dictionary<int, IEnumerable<SelectListItem>> variablesLists = new Dictionary<int, IEnumerable<SelectListItem>>();
            var inputsOutputs = _ioRepository.GetBySystemId(systemId);
            foreach(var item in inputsOutputs)
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Value = "-1", Text = "Not set" });
                items.AddRange(item.Variables.OrderBy(v => v.VariableId).Select(v => new SelectListItem { Value = v.VariableId.ToString(), Text = v.Name }));
                variablesLists.Add(item.InputOutputId, items);
            }
            return variablesLists;
        }

        public Dictionary<int, List<VariableChartData>> CreateChartsData(int systemId)
        {
            Dictionary<int, List<VariableChartData>>  chartsData = new Dictionary<int, List<VariableChartData>>();
            var inputs = _ioRepository.GetBySystemId(systemId).Where(io => io.Type == IOType.Input);
            foreach(var item in inputs)
            {
                List<VariableChartData> data = new List<VariableChartData>();
                foreach(var variable in item.Variables)
                {
                    data.Add(new VariableChartData
                    {
                         Data = variable.Data,
                         VariableId = variable.VariableId,
                         Type = variable.Type
                    });
                }
                chartsData.Add(item.InputOutputId, data);
            }
            return chartsData;
        }

        public RuleVM CreateNewRule(int systemId, int ruleId)
        {
            RuleVM rule = new RuleVM { RuleId = ruleId, SystemId = systemId, RuleElements = new List<RuleElementVM>() };
            foreach(var io in _ioRepository.GetBySystemId(systemId).OrderBy(io => io.Type))
            {
                rule.RuleElements.Add(new RuleElementVM
                {
                    ElementId = -1,
                    Type = io.Type == IOType.Input ? RuleElementType.InputPart : RuleElementType.OutputPart,
                    InputOutputId = io.InputOutputId,
                    VariableId = -1,
                    InputOutputName = io.Name,
                    VariableName = string.Empty,
                    IsNegation = false,
                    NextOperation = RuleNextOperation.And
                });
            }
            return rule;
        }

        public void Save(List<RuleVM> rules)
        {
            _saver.Save(rules);
        }
    }
}
