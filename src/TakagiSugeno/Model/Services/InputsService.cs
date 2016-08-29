using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.ViewModels;
using Newtonsoft.Json;

namespace TakagiSugeno.Model.Services
{
    public class InputsService
    {
        private IRepository<InputOutput> _repository;

        public InputsService(IRepository<InputOutput> repository)
        {
            _repository = repository;
        }

        public List<InputVM> GetSystemInputs(int systemId)
        {
            return _repository.GetBySystemId(systemId).Where(i => i.Type == IOType.Input)
                .Select(i => new InputVM { Name = i.Name, InputId = i.InputOutputId}).ToList();               
        }

        public InputVM GetInput(int inputId)
        {
            return MapEntityToVM(_repository.GetById(inputId));
        }
        private InputVM MapEntityToVM(InputOutput entity)
        {
            InputVM vm = new InputVM { Name = entity.Name, InputId = entity.InputOutputId, Variables = new List<VariableVM>() };
            vm.Variables.AddRange(entity.Variables.Select(v => new VariableVM
            {
                Name = v.Name,
                Type = v.Type,
                VariableId = v.VariableId,
                FunctionData = JsonConvert.DeserializeObject<Dictionary<string, double>>(v.Data)
            }));
            return vm;
        }

    }
}
