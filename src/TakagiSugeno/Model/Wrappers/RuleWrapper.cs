using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public class RuleWrapper
    {
        public List<MembershipDegree> MembershipDegrees { get; set; }
        public double CalculatedValue { get; set; }
        public Rule Rule { get; set; }
        public string RuleLog { get; set; }
        public string LogPrefix { get; set; }

        public string RuleMemebershipInfo()
        {
            StringBuilder res = new StringBuilder();
            foreach(MembershipDegree degree in MembershipDegrees)
            {
                string nextOperation = degree.NextOperation != RuleNextOperation.None ? degree.NextOperation.ToString() + " " : string.Empty;
                res.Append($"{degree.Value} {nextOperation}");
            }

            res.Append($" = {CalculatedValue}");

            return res.ToString();
        }

        public string RuleInfo()
        {
            StringBuilder info = new StringBuilder("If ");
            foreach(RuleElement elem in Rule.RuleElements)
            {             
                string nextOperation = elem.NextOpartion != RuleNextOperation.None ? elem.NextOpartion.ToString().ToLower() + " " : "then ";
                string negation = elem.IsNegation ? " not" : string.Empty;
                if (elem.Type == RuleElementType.InputPart)
                {
                    info.Append($"{elem.InputOutput?.Name} is{negation} {elem.Variable?.Name} {nextOperation}");
                }
                else
                {
                    string and = Rule.RuleElements.IndexOf(elem) != Rule.RuleElements.Count - 1 ? "and " : string.Empty;
                    info.Append($"{elem.InputOutput?.Name} is {elem.Variable?.Name} {and}");
                }
            }

            return info.ToString();
        }
    }

    public class MembershipDegree
    {
        public double Value { get; set; }
        public RuleNextOperation NextOperation { get; set; }
    }
}
