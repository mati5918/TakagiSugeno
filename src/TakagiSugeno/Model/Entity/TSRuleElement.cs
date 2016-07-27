using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class TSRuleElement
    {
        public int TSRuleElementId { get; set; }
        public RuleElementType Type { get; set; }
      
        public int TSInputOutputId { get; set; }
        public TSInputOutput InputOutput { get; set; }

        public int TSVariableId { get; set; }
        public TSVariable Variable { get; set; }

        public int TSRuleId { get; set; }
        public TSRule Rule { get; set; }
    }
}
