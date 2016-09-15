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
            _outputsRepository.Delete(_outputsRepository.GetById(id));
        }

        public SaveResult Save(OutputVM viewModel)
        {
            //if (IsInputValid(viewModel))
            //{
                _saver.Save(viewModel);
            //}
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

    }
}
