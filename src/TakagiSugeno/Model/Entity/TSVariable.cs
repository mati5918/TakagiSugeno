using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class TSVariable
    {
        public int TSVariableId { get; set; }
        public string Name { get; set; }
        public VariableType Type { get; set; }
        public string Data { get; set; }

        public int TSInputOutputId { get; set; }
        public TSInputOutput InputOutput { get; set; }

    }
}
