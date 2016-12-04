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
    public class VariablesService
    {
        private IRepository<Variable> _repository;
        private InputsService _inputsService;

        public VariablesService(IRepository<Variable> repository, InputsService service)
        {
            _repository = repository;
            _inputsService = service;
        }

        public void ChangeVariableType(VariableVM variable, int systemId = -1)
        {
            if (variable != null)
            {
                Dictionary<string, double> newData = GetVariableNewData(variable.Type, systemId);
                List<string> keys = new List<string>(newData.Keys);
                /*if (newData != null)
                {
                    foreach (var item in keys)
                    {
                        if (variable.FunctionData.ContainsKey(item))
                        {
                            newData[item] = variable.FunctionData[item];
                        }
                    }
                }*/
                variable.FunctionData = newData;
            }
        }

        public VariableVM CreateVariable(int fakeId, IOType type)
        {
            if(type == IOType.Input)
            {
                return CreateInputVariable(fakeId);
            }
            else if (type == IOType.Output)
            {
                return CreateOutputVariable(fakeId);
            }
            else
            {
                throw new ArgumentException("Wrong IO type");
            }
        }

        private VariableVM CreateInputVariable(int fakeId)
        {
            return new VariableVM
            {
                VariableId = fakeId,
                Type = Model.VariableType.Triangle,
                FunctionData = NewTriangleData()
            };
        }

        private VariableVM CreateOutputVariable(int fakeId)
        {
            return new VariableVM
            {
                VariableId = fakeId,
                Type = VariableType.OutputConst,
                FunctionData = NewOutputConstData()
            };
        }

        public void RemoveVariable(InputVM viewModel, int variableId)
        {
            VariableVM variable = viewModel.Variables.FirstOrDefault(v => v.VariableId == variableId);
            if (variable != null)
            {
                viewModel.Variables.Remove(variable);
            }
        }

        private Dictionary<string, double> GetVariableNewData(VariableType type, int systemId)
        {
            switch(type)
            {
                case VariableType.Triangle: return NewTriangleData();
                case VariableType.Trapeze: return NewTrapezeData();
                case VariableType.Gaussian: return NewGaussianData();
                case VariableType.OutputConst: return NewOutputConstData();
                case VariableType.OutputFunction: return NewOutputFunctionData(systemId);
                default: return null;
            }
        }

        private Dictionary<string, double> NewTriangleData()
        {
            return new Dictionary<string, double> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
        }

        private Dictionary<string, double> NewTrapezeData()
        {
            return new Dictionary<string, double> { { "a", 1 }, { "b", 2 }, { "c", 3 }, {"d", 4 } };
        }

        private Dictionary<string, double> NewGaussianData()
        {
            return new Dictionary<string, double> { { "sigma", 1 }, { "c", 2 } };
        }

        private Dictionary<string, double> NewOutputConstData()
        {
            return new Dictionary<string, double> { { "wartość", 0 }};
        }

        private Dictionary<string, double> NewOutputFunctionData(int systemId)
        {
            List<string> names = _inputsService.GetSystemInputsNames(systemId);
            Dictionary<string, double> res = new Dictionary<string, double>();
            foreach(string name in names)
            {
                res.Add(name, 0);
            }
            return res;
        }

    }
}
