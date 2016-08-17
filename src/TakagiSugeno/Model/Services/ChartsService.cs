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
    public class ChartsService
    {
        private IRepository<Variable> _repository;

        public ChartsService(IRepository<Variable> repository)
        {
            _repository = repository;
        }

        public List<VariableVM> GetChartData(int inputId)
        {
            List<VariableVM> result = new List<VariableVM>();
            var items = _repository.Get(v => v.InputOutputId == inputId);
            foreach(var item in items)
            {
                result.Add(new VariableVM
                {
                    Name = item.Name,
                    VariableId = item.VariableId,
                    Data = GetVariableData(item)
                });
            }
            return result;
        }

        private List<ChartData> GetVariableData(Variable item)
        {
            Dictionary<string, double> data = JsonConvert.DeserializeObject<Dictionary<string, double>>(item.Data);
            List<ChartData> results = new List<ChartData>();
            switch(item.Type)
            {
                case VariableType.Triangle:
                    results.Add(new ChartData { X = data["a"], Y = 0 });
                    results.Add(new ChartData { X = data["b"], Y = 1 });
                    results.Add(new ChartData { X = data["c"], Y = 0 });
                    break;
                case VariableType.Trapeze:
                    results.Add(new ChartData { X = data["a"], Y = 0 });
                    results.Add(new ChartData { X = data["b"], Y = 1 });
                    results.Add(new ChartData { X = data["c"], Y = 1 });
                    results.Add(new ChartData { X = data["d"], Y = 0 });
                    break;
            }
            return results;
        }
    }


}
