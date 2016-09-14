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

        public OutputsService(IRepository<InputOutput> repository)
        {
            _outputsRepository = repository;
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
    }
}
