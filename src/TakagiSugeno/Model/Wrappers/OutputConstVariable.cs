using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public class OutputConstVariable : IOutputVariable
    {
        public Variable Variable { get; }
        private Dictionary<string, double> data = new Dictionary<string, double>();

        public OutputConstVariable(Variable variable)
        {
            this.Variable = variable;
            data = JsonConvert.DeserializeObject<Dictionary<string, double>>(variable.Data);
        }

        public double GetValue(Dictionary<string, double> inputs = null)
        {
            return data.FirstOrDefault().Value;
        }
    }
}
