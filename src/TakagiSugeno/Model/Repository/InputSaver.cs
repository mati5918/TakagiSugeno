using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Repository
{
    public class InputSaver
    {
        private TakagiSugenoDbContext _context;
        public InputSaver(TakagiSugenoDbContext context)
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
                SaveVariables(viewModel);
                _context.SaveChanges();

            }
        }

        private void SaveVariables(InputVM viewModel)
        {
            IEnumerable<Variable> variableEntities = _context.Variables.Where(v => v.InputOutputId == viewModel.InputId);
            IEnumerable<VariableVM> existedVariables = viewModel.Variables.Where(v => v.VariableId >= 0);
            IEnumerable<VariableVM> newVariables = viewModel.Variables.Where(v => v.VariableId < 0);
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
                    InputOutputId = viewModel.InputId
                };
                _context.Variables.Add(newVariable);
            }
        }
    }
}
