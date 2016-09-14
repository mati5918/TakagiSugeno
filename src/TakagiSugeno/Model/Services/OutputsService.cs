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
        private IRepository<InputOutput> _repository;

        public OutputsService(IRepository<InputOutput> repository)
        {
            _repository = repository;
        }

        public List<OutputVM> GetSystemOutputs(int systemId)
        {
            return _repository.GetBySystemId(systemId).Where(i => i.Type == IOType.Output)
                .Select(i => new OutputVM
                {
                    Name = i.Name,
                    OutputId = i.InputOutputId,
                    SystemId = i.TSSystemId,
                    Variables = i.Variables.Select(v => new VariableVM { Type = v.Type, JsonData = v.Data }).ToList()
                }).ToList();
        }
    }
}
