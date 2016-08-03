using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Wrappers;

namespace TakagiSugeno.Model
{
    public class OutputCalculator
    {
        private List<Rule> _rules;
        public List<VariableWrapper> Variables { get; set; }
        public Dictionary<int, double> InputValues { get; set; }

        public List<double> CalcRuleMembershipDegrees(Rule rule)
        {
            List<double> degrees = new List<double>();
            foreach (RuleElement elem in rule.RuleElements)
            {
                VariableWrapper variable = Variables.FirstOrDefault(v => v.InputId == elem.InputOutputId && v.VariableId == elem.VariableId);
                if(variable != null)
                {
                    double inputValue = InputValues[variable.InputId];
                    degrees.Add(variable.MembershipFunction.CalcMembership(inputValue));
                }
                else
                {
                    throw new Exception("Variable not found");
                }
            }
            return degrees;
        }
    }
}
