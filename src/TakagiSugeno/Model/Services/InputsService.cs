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


    }
}
