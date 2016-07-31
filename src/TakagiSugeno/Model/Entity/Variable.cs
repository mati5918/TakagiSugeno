using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class Variable
    {
        public int VariableId { get; set; }
        public string Name { get; set; }
        public VariableType Type { get; set; }
        public string Data { get; set; }

        public int InputOutputId { get; set; }
        public InputOutput InputOutput { get; set; }

    }
}
