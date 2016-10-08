using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class OutputCalcVM
    {
        public int SystemId { get; set; }
        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }
        
    }

    public class OutputCalcData
    {
        public int SystemId { get; set; }
        public Dictionary<string, double> InputsValues { get; set; }
    }
}
