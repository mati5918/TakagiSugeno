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

        public RulesService(IRepository<Rule> ruleRepository, IRepository<InputOutput> ioRepository)
        {
            _ruleRepository = ruleRepository;
            _ioRepository = ioRepository;
        }

        public RuleGeneralVM GetSystemRules(int systemId)
        {
            RuleGeneralVM res = new RuleGeneralVM();
            res.Rules = _ruleRepository.GetBySystemId(systemId)
                .Select(r => new RuleVM
                {
                    RuleId = r.RuleId,
                    SystemId = r.TSSystemId,
                    RuleElements = r.RuleElements
                .Select(elem => new RuleElementVM
                {
                    ElementId = elem.RuleElementId,
                    Type = elem.Type,
                    InputOutputId = elem.InputOutputId,
                    VariableId = elem.VariableId,
                    InputOutputName = elem.InputOutput.Name,
                    VariableName = elem.Variable.Name
                }).ToList()
                }).ToList();
            CreateVariablesLists(res, systemId);
            FillChartsData(res, systemId);
            return res;
        }

        public List<string> GetSystemInputsOutputsNames(int systemId)
        {
            return _ioRepository.GetBySystemId(systemId).OrderBy(io => io.Type).Select(io => io.Name).ToList();
        }

        private void CreateVariablesLists(RuleGeneralVM vm, int systemId)
        {
            vm.VariablesLists = new Dictionary<int, IEnumerable<SelectListItem>>();
            var inputsOutputs = _ioRepository.GetBySystemId(systemId);
            foreach(var item in inputsOutputs)
            {
                vm.VariablesLists.Add(item.InputOutputId, item.Variables.Select(v => new SelectListItem { Value = v.VariableId.ToString(), Text = v.Name }));
            }

        }

        private void FillChartsData(RuleGeneralVM vm, int systemId)
        {
            vm.ChartsData = new Dictionary<int, Dictionary<int, string>>();
            var inputs = _ioRepository.GetBySystemId(systemId).Where(io => io.Type == IOType.Input);
            foreach(var item in inputs)
            {
                Dictionary<int, string> data = new Dictionary<int, string>();
                foreach(var variable in item.Variables)
                {
                    data.Add(variable.VariableId, variable.Data);
                }
                vm.ChartsData.Add(item.InputOutputId, data);
            }
        }
    }
}
