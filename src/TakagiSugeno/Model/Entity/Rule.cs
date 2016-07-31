using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class Rule
    {
        public int RuleId { get; set; }

        public int TSSystemId { get; set; }
        public TSSystem System { get; set; }
        public List<RuleElement> RuleElements { get; set; }
    }
}
