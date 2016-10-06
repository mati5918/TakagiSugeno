using Microsoft.EntityFrameworkCore;
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
        public Dictionary<int, double> InputValues { get; set; }

        private TakagiSugenoDbContext _context;
        //private List<InputOutput> _outputs;
        private List<RuleWrapper> _ruleWrappers = new List<RuleWrapper>();
        private List<InputVariableWrapper> _inputVariablesWrappers = new List<InputVariableWrapper>();

        public OutputCalculator(TakagiSugenoDbContext context)
        {
            _context = context;
        }
        public void CalcOutputsValues(int systemId)
        {
            _inputVariablesWrappers = _context
                .Variables
                .Where(v => v.InputOutput.TSSystemId == systemId)
                .Select(v => new InputVariableWrapper(v))
                .ToList();
              
            _ruleWrappers = _context
                .Rules
                .Include(r => r.RuleElements)
                .Where(r => r.TSSystemId == systemId)
                .Select(r => new RuleWrapper()
                {
                    Rule = r,
                    MembershipDegrees = CalcRuleMembershipDegrees(r)
                    //CalculatedValue = PerformRuleOperation(this)
                })
                .ToList();

            PerformRulesOperations();

            /*foreach (InputOutput output in _outputs)
            {

            }*/
        }

        private void PerformRulesOperations()
        {
            foreach(RuleWrapper val in _ruleWrappers)
            {
                val.CalculatedValue = PerformRuleOperation(val);
            }
        }

        private double PerformRuleOperation(RuleWrapper rule)
        {
            if (rule.MembershipDegrees.Count == 1)
            {
                return rule.MembershipDegrees[0].Value;
            }
            else
            {
                double res = rule.MembershipDegrees[0].Value;
                RuleNextOperation operation = rule.MembershipDegrees[0].NextOperation;
                for (int i = 1; i < rule.MembershipDegrees.Count; i++)
                {
                    if(operation == RuleNextOperation.And)
                    {
                        res = PerfromAndOpeation(res, rule.MembershipDegrees[i].Value);
                    }
                    else if(operation == RuleNextOperation.Or)
                    {
                        res = PerfromOrOpeation(res, rule.MembershipDegrees[i].Value);
                    }
                    operation = rule.MembershipDegrees[i].NextOperation;
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

        /*private void CalcRulesMembershipDegrees()
        {
            _ruleWrappers.Clear();
            foreach (Rule r in SystemRules)
            {
                RuleWrapper ruleWrapper = new RuleWrapper
                {
                    Rule = r,
                    MembershipDegrees = CalcRuleMembershipDegrees(r)
                };
            }
        }*/

        public List<MembershipDegree> CalcRuleMembershipDegrees(Rule rule) //TODO not-set variable
        {
            List<MembershipDegree> degrees = new List<MembershipDegree>();
            foreach (RuleElement elem in rule.RuleElements.Where(e => e.Type == RuleElementType.InputPart))
            {
                InputVariableWrapper variable = _inputVariablesWrappers.FirstOrDefault(v => v.InputId == elem.InputOutputId && v.VariableId == elem.VariableId);
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

    }

    

    
}
