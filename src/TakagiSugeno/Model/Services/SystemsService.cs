using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Services
{
    public class SystemsService
    {
        private IRepository<TSSystem> _repository;
        public SystemsService(IRepository<TSSystem> repository)
        {
            _repository = repository;
        }

        public AndMethod GetSystemAndMethod(int systemId)
        {
            return _repository.GetById(systemId).AndMethod;
        }

        public OrMethod GetSystemOrMethod(int systemId)
        {
            return _repository.GetById(systemId).OrMethod;
        }

        public void ModifySystemMethods(int systemId, AndMethod andMethod, OrMethod orMethod)
        {
            TSSystem system = _repository.GetById(systemId);
            system.AndMethod = andMethod;
            system.OrMethod = orMethod;
            _repository.Update(system);
        }

        public List<SystemVM> GetSystems()
        {
            //TODO where IsPublished
            return _repository.GetAll().Select(s => new SystemVM
            {
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate,
                Description = s.Description,
                Name = s.Name,
                PublishedDate = s.PublishedDate,
                TSSystemId = s.TSSystemId
            }).ToList();
        }
    }
}
