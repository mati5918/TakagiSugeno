using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Services
{
    public class OutputsService
    {
        private IRepository<InputOutput> _outputsRepository;
        private InputOutputSaver _saver;

        private List<string> validationErros = new List<string>();

        public OutputsService(IRepository<InputOutput> repository, InputOutputSaver saver)
        {
            _outputsRepository = repository;
            _saver = saver;
        }

        public List<OutputVM> GetSystemOutputs(int systemId)
        {
            return _outputsRepository.GetBySystemId(systemId).Where(i => i.Type == IOType.Output)
                .Select(i => new OutputVM
                {
                    Name = i.Name,
                    OutputId = i.InputOutputId,
                    SystemId = i.TSSystemId,
                    Variables = i.Variables.Select(v => new VariableVM { Type = v.Type, Name = v.Name }).ToList()
                }).ToList();
        }

        public List<string> GetSystemOutputsNames(int systemId)
        {
            return _outputsRepository.GetBySystemId(systemId).Where(o => o.Type == IOType.Output)
                .Select(o => o.Name).ToList();
        }

        public OutputVM GetOutput(int outputId)
        {
            return MapEntityToVM(_outputsRepository.GetById(outputId));
        }
        private OutputVM MapEntityToVM(InputOutput entity)
        {
            OutputVM vm = new OutputVM { Name = entity.Name, OutputId = entity.InputOutputId, Variables = new List<VariableVM>(), SystemId = entity.TSSystemId };
            vm.Variables.AddRange(entity.Variables.Select(v => new VariableVM
            {
                Name = v.Name,
                Type = v.Type,
                VariableId = v.VariableId,
                FunctionData = JsonConvert.DeserializeObject<Dictionary<string, double>>(v.Data)
            }));
            return vm;
        }

        public void Remove(int id)
        {
            InputOutput item = _outputsRepository.GetById(id);
            _saver.RemoveRuleElements(item);
            _outputsRepository.Delete(item);
        }

        public SaveResult Save(OutputVM viewModel)
        {
            if (IsOutputValid(viewModel))
            {
                _saver.Save(viewModel);
            }
            return new SaveResult { Id = viewModel.OutputId, Errors = validationErros };
        }

        public OutputVM AddOutput(int systemId)
        {
            return new OutputVM
            {
                OutputId = -1,
                SystemId = systemId,
                Variables = new List<VariableVM>()
            };
        }

        #region validation
        public bool IsOutputValid(OutputVM output)
        {
            validationErros.Clear();
            ValidateOutputName(output);
            ValidateVariables(output);
            validationErros = validationErros.Distinct().ToList();
            return validationErros.Count > 0 ? false : true;
        }

        private void ValidateOutputName(OutputVM output)
        {
            string name = output.Name;
            bool isValid = Tools.Tools.IsNameValid(name, "wyjścia", validationErros);
            if (isValid && _outputsRepository.GetBySystemId(output.SystemId).Any(i => i.Name.ToUpper() == name.ToUpper() && i.InputOutputId != output.OutputId && i.Type == IOType.Output))
            {
                validationErros.Add("Nazwa wyjścia musi być unikalna dla systemu");
                return;
            }
        }

        private void ValidateVariableName(VariableVM variable, List<string> names)
        {
            string name = variable.Name;
            bool isValid = Tools.Tools.IsNameValid(name, "zmiennej", validationErros);
            if (isValid && names.Count(n => n.ToUpper() == name.ToUpper()) > 1)
            {
                validationErros.Add($"Nazwa zmiennej {name.ToUpper()} musi być unikalna dla wyjścia");
                return;
            }
        }

        private void ValidateVariables(OutputVM output)
        {
            if (output.Variables == null || output.Variables.Count == 0)
            {
                validationErros.Add("Wyjście musi posiadać conajmniej jedną zmienną");
                return;
            }
            List<string> names = output.Variables.Select(v => v.Name).ToList();
            foreach (var variable in output.Variables)
            {
                ValidateVariableName(variable, names);
            }
        }
        #endregion

    }
}
