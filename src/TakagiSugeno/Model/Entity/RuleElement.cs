using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class RuleElement
    {
        public int RuleElementId { get; set; }
        public RuleElementType Type { get; set; }
        public RuleNextOperation NextOpartion { get; set; }
        public bool IsNegation { get; set; }

        public int InputOutputId { get; set; }
        public InputOutput InputOutput { get; set; }

        public int? VariableId { get; set; }
        public Variable Variable { get; set; }

        public int RuleId { get; set; }
        public Rule Rule { get; set; }
    }
}
