using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public class OutputFunctionVariable : IOutputVariableFunction
    {
        private Dictionary<string, double> data = new Dictionary<string, double>();

        public OutputFunctionVariable(Variable variable)
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, double>>(variable.Data);
        }

        public double GetValue(Dictionary<string, double> inputs = null)
        {
            double res = 0;
            foreach(var item in data)
            {
                double inputValue;
                if(inputs.TryGetValue(item.Key, out inputValue))
                {
                    res += (item.Value * inputValue);
                }
                else
                {
                    throw new Exception("incorrect input data");
                }
            }
            return res;
        }
    }
}
