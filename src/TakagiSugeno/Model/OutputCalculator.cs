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
        
        public AndMethod AndMethod { get; set; }
        public OrMethod OrMethod { get; set; }
        public List<VariableWrapper> Variables { get; set; }
        public Dictionary<int, double> InputValues { get; set; }
        public List<Rule> SystemRules { get; set; }

        private List<InputOutput> outputs;
        private List<RuleValue> ruleValues = new List<RuleValue>();
        //private List<Rule> _rules;

        public void Calc()
        {
            CalcRulesMembershipDegrees();
            PerformRulesOperations();
            foreach(InputOutput output in outputs)
            {

            }
        }

        private void PerformRulesOperations()
        {
            foreach(RuleValue val in ruleValues)
            {
                val.Value = PerformRuleOperation(val);
            }
        }

        private double PerformRuleOperation(RuleValue ruleValue)
        {
            if (ruleValue.MembershipDegrees.Count == 1)
            {
                return ruleValue.MembershipDegrees[0].Value;
            }
            else
            {
                double res = ruleValue.MembershipDegrees[0].Value;
                RuleNextOperation operation = ruleValue.MembershipDegrees[0].NextOperation;
                for (int i = 1; i < ruleValue.MembershipDegrees.Count; i++)
                {
                    if(operation == RuleNextOperation.And)
                    {
                        res = PerfromAndOpeation(res, ruleValue.MembershipDegrees[i].Value);
                    }
                    else if(operation == RuleNextOperation.Or)
                    {
                        res = PerfromOrOpeation(res, ruleValue.MembershipDegrees[i].Value);
                    }
                    operation = ruleValue.MembershipDegrees[i].NextOperation;
                }
                return res;
            }
        }

        private double PerfromAndOpeation(double res, double val)
        {
            switch(this.AndMethod)
            {
                case AndMethod.Product: return res * val;
                case AndMethod.Min: return Math.Min(res, val);
            }
            return res;
        }

        private double PerfromOrOpeation(double res, double val)
        {
            throw new NotImplementedException();
        }

        private void CalcRulesMembershipDegrees()
        {
            ruleValues.Clear();
            foreach (Rule r in SystemRules)
            {
                RuleValue ruleValue = new RuleValue
                {
                    RuleId = r.RuleId,
                    MembershipDegrees = CalcRuleMembershipDegrees(r)
                };
            }
        }

        private List<MembershipDegree> CalcRuleMembershipDegrees(Rule rule)
        {
            List<MembershipDegree> degrees = new List<MembershipDegree>();
            foreach (RuleElement elem in rule.RuleElements.Where(e => e.Type == RuleElementType.InputPart))
            {
                VariableWrapper variable = Variables.FirstOrDefault(v => v.InputId == elem.InputOutputId && v.VariableId == elem.VariableId);
                if(variable != null)
                {
                    double inputValue = InputValues[variable.InputId];
                    degrees.Add(new MembershipDegree
                    {
                        Value = variable.MembershipFunction.CalcMembership(inputValue),
                        NextOperation = elem.NextOpartion
                    });
                }
                else
                {
                    throw new Exception("Variable not found");
                }
            }
            return degrees;
        }


        private class RuleValue
        {
            public int RuleId { get; set; }
            public List<MembershipDegree> MembershipDegrees { get; set; }
            public double Value { get; set; }
        }

        private class MembershipDegree
        {
            public double Value { get; set; }
            public RuleNextOperation NextOperation { get; set; }
        }

    }

    

    
}
