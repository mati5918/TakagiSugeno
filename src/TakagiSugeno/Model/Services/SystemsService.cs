using Microsoft.AspNetCore.Http;
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
        private SystemCloner _cloner;
        public SystemsService(IRepository<TSSystem> repository, SystemCloner cloner)
        {
            _repository = repository;
            _cloner = cloner;
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
            return _repository.GetAll().Where(s => s.PublishedDate.Month == 12).Select(s => new SystemVM
            {
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate,
                Description = s.Description,
                Name = s.Name,
                PublishedDate = s.PublishedDate,
                TSSystemId = s.TSSystemId
            }).ToList();
        }

        public void Publish(PublishVM publishData)
        {
            TSSystem system = _repository.GetById(publishData.SystemId);
            system.IsPublished = true;
            system.PublishedDate = DateTime.Now;
            system.CreatedBy = publishData.Author;
            system.Description = publishData.Description;
            system.Name = publishData.Name;
            _repository.Update(system);
        }

        public int CreateSystem()
        {
            TSSystem system = new TSSystem
            {
                AndMethod = AndMethod.Product,
                OrMethod = OrMethod.Max,
                CreatedDate = DateTime.Now,
                IsPublished = false
            };
            _repository.Add(system);
            return system.TSSystemId;
        }

        public int CloneSystem(int systemId)
        {
            return _cloner.CloneSystem(systemId);
        }

        public string SystemToJson(int systemId)
        {
            return _cloner.SystemToJson(systemId);
        }

        public int? ReadFromFile(IFormFile file)
        {
            if(file.ContentType == "application/json")
            {
                return _cloner.ReadFromFile(file);
            }
            return null;
        }

    }
}
