using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class InputOutput
    {
        public int InputOutputId { get; set; }
        public string Name { get; set; }
        public IOType Type { get; set; }
        public double RangeStart { get; set; }
        public double RangeEnd { get; set; }

        public List<Variable> Variables { get; set; }
        public int TSSystemId { get; set; }
        public TSSystem System { get; set; }
    }
}
