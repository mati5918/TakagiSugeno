using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class VariableVM
    {
        public int VariableId { get; set; }
        public string Name { get; set; }
        public string JsonData { get; set; }
        public Dictionary<string, double> FunctionData { get; set; }
        public VariableType Type { get; set; }
    }

}
