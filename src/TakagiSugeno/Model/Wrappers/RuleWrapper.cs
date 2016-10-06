using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public class RuleWrapper
    {
        public List<MembershipDegree> MembershipDegrees { get; set; }
        public double CalculatedValue { get; set; }
        public Rule Rule { get; set; }
    }

    public class MembershipDegree
    {
        public double Value { get; set; }
        public RuleNextOperation NextOperation { get; set; }
    }
}
