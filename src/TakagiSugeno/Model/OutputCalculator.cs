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
        private List<VariableWrapper> _variables;
        private Dictionary<string, double> _inputValues;

        private List<double> CalcRuleMembershipDegrees(Rule rule)
        {
            List<double> degrees = new List<double>();
            foreach(VariableWrapper variable in _variables)
            {
                if(rule.RuleElements.Any(elem => elem.InputOutput.Name == variable.InputName && elem.Variable.Name == variable.VariableName))
                {
                    double inputValue = _inputValues[variable.InputName];
                    degrees.Add(variable.MembershipFunction.CalcMembership(inputValue));
                }
            }
            return degrees;
        }
    }
}
