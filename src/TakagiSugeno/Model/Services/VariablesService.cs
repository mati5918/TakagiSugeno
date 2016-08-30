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

        public VariablesService(IRepository<Variable> repository)
        {
            _repository = repository;
        }

        public void ChangeVariableType(VariableVM variable)
        {
            if (variable != null)
            {
                ModifyVariableType(variable);
            }
        }

        public VariableVM CreateVariable()
        {
            return new VariableVM
            {
                VariableId = CreateFakeVariableId(),
                Type = Model.VariableType.Triangle,
                FunctionData = new Dictionary<string, double> { { "a", 0 }, { "b", 0 }, { "c", 0 } }
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

        private void ModifyVariableType(VariableVM variable)
        {
            Dictionary<string, double> newData = GetVariableNewData(variable.Type);
            List<string> keys = new List<string>(newData.Keys);
            if(newData != null)
            {
                foreach(var item in keys)
                {
                    if(variable.FunctionData.ContainsKey(item))
                    {
                        newData[item] = variable.FunctionData[item];
                    }
                }
            }
            variable.FunctionData = newData;
        }

        private Dictionary<string, double> GetVariableNewData(VariableType type)
        {
            switch(type)
            {
                case VariableType.Triangle: return GetNewTriangleFunctionData();
                case VariableType.Trapeze: return GetNewTrapezeFunctionData();
                default: return null;
            }
        }

        private Dictionary<string, double> GetNewTriangleFunctionData()
        {
            return new Dictionary<string, double> { { "a", 0 }, { "b", 0 }, { "c", 0 } };
        }

        private Dictionary<string, double> GetNewTrapezeFunctionData()
        {
            return new Dictionary<string, double> { { "a", 0 }, { "b", 0 }, { "c", 0 }, {"d", 0 } };
        }

        private int CreateFakeVariableId()
        {
            /*if (viewModel.Variables.Count == 0) return -1;
            int res = 0;
            int minId = viewModel.Variables.Min(v => v.VariableId);
            res = minId >= 0 ? -1 : minId - 1;
            return res;*/
            return -1;
        }
    }
}
