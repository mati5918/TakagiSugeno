using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.ViewModels;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace TakagiSugeno.Model.Services
{
    public class InputsService
    {
        private IRepository<InputOutput> _inputRepository;
        private IRepository<Variable> _variableRepository;
        //private VariablesService _variableService;
        private InputOutputSaver _saver;

        private List<string> validationErros = new List<string>();

        public InputsService(IRepository<InputOutput> inputRepository, IRepository<Variable> variableRepository, InputOutputSaver saver)
        {
            _inputRepository = inputRepository;
            _variableRepository = variableRepository;
            _saver = saver;
        }

        public List<InputVM> GetSystemInputs(int systemId)
        {
            return  _inputRepository.GetBySystemId(systemId).Where(i => i.Type == IOType.Input)
                .Select(i => new InputVM { Name = i.Name, InputId = i.InputOutputId, SystemId = i.TSSystemId,
                    Variables = i.Variables.Select(v => new VariableVM { Type = v.Type, JsonData = v.Data}).ToList()}).ToList();         
        }

        public InputVM GetInput(int inputId)
        {
            return MapEntityToVM(_inputRepository.GetById(inputId));
        }
        private InputVM MapEntityToVM(InputOutput entity)
        {
            InputVM vm = new InputVM { Name = entity.Name, InputId = entity.InputOutputId, Variables = new List<VariableVM>(), SystemId = entity.TSSystemId };
            vm.Variables.AddRange(entity.Variables.Select(v => new VariableVM
            {
                Name = v.Name,
                Type = v.Type,
                VariableId = v.VariableId,
                FunctionData = JsonConvert.DeserializeObject<Dictionary<string, double>>(v.Data)
            }));
            return vm;
        }

        public SaveResult Save(InputVM viewModel)
        {
            if (IsInputValid(viewModel))
            {
                _saver.Save(viewModel);
            }
            return new SaveResult { Id = viewModel.InputId, Errors = validationErros};
        }

        internal void Remove(int id)
        {
            _inputRepository.Delete(_inputRepository.GetById(id));
        }

        
        public bool IsInputValid(InputVM input)
        {
            validationErros.Clear();
            ValidateInputName(input);
            ValidateVariables(input);
            validationErros = validationErros.Distinct().ToList();
            return validationErros.Count > 0 ? false : true;
        }

        public InputVM AddInput(int systemId)
        {
            return new InputVM
            {
                InputId = -1,
                SystemId = systemId,
                Variables = new List<VariableVM>()
            };
        }

        private void ValidateInputName(InputVM input)
        {
            string name = input.Name;
            bool isValid = IsNameValid(name, "wejścia");
            if (isValid && _inputRepository.GetBySystemId(input.SystemId).Any(i => i.Name.ToUpper() == name.ToUpper() && i.InputOutputId != input.InputId && i.Type == IOType.Input))
            {
                validationErros.Add("Nazwa wejścia musi być unikalna dla systemu");
                return;
            }
        }

        private void ValidateVariableName(VariableVM variable, List<string> names)
        {
            string name = variable.Name;
            bool isValid = IsNameValid(name, "zmiennej");
            if (isValid && names.Count( n => n.ToUpper() == name.ToUpper()) > 1)
            {
                validationErros.Add($"Nazwa zmiennej {name.ToUpper()} musi być unikalna dla wejścia");
                return;
            }
        }

        private bool IsNameValid(string name, string type)
        {
            if (string.IsNullOrEmpty(name))
            {
                validationErros.Add($"Nazwa {type} nie może być pusta");
                return false;
            }
            if (name.Length > 30)
            {
                validationErros.Add($"Nazwa {type} nie może być dłuższa niż 30 znaków");
                return false;
            }
            if (!Regex.IsMatch(name, @"^[-a-zA-Z0-9]*$"))
            {
                validationErros.Add($"Nazwa {type} zawiera niedozwolone znaki");
                return false;
            }
            return true;
        }

        private void ValidateVariables(InputVM input)
        {
            if(input.Variables == null || input.Variables.Count == 0)
            {
                validationErros.Add("Wejście musi posiadać conajmniej jedną zmienną");
                return;
            }
            List<string> names = input.Variables.Select(v => v.Name).ToList();
            foreach(var variable in input.Variables)
            {
                ValidateVariableName(variable, names);
                ValidateFunctionData(variable);
            }
        }

        private void ValidateFunctionData(VariableVM variable)
        {
            string name = string.IsNullOrEmpty(variable.Name) ? string.Empty : $" {variable.Name}";
            string err = $"Złe dane funkcji{name}.";
            if (variable.Type == VariableType.Triangle)
            {
                double a = variable.FunctionData["a"];
                double b = variable.FunctionData["b"];
                double c = variable.FunctionData["c"];
                if(!(c > b && b > a))
                {
                    validationErros.Add($"{err} Dla funkcji trójkątnej musi być spełniony warunek c > b > a");
                }
            }
            if (variable.Type == VariableType.Trapeze)
            {
                double a = variable.FunctionData["a"];
                double b = variable.FunctionData["b"];
                double c = variable.FunctionData["c"];
                double d = variable.FunctionData["d"];
                if (!(d > c && c > b && b > a))
                {
                    validationErros.Add($"{err} Dla funkcji trapezoidalnej musi być spełniony warunek d > c > b > a");
                }
            }
        }
    }
}
