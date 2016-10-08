using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.ViewModels;
using TakagiSugeno.Model.Wrappers;

namespace TakagiSugeno.Model
{
    public class OutputCalculator
    {
        
        
        //public Dictionary<int, double> InputValues { get; set; }

        private TakagiSugenoDbContext _context;
        //private List<InputOutput> _outputs;
        private List<RuleWrapper> _ruleWrappers = new List<RuleWrapper>();
        private List<InputVariableWrapper> _inputVariablesWrappers = new List<InputVariableWrapper>();
        private List<IOutputVariable> _outputVariablesWrappers = new List<IOutputVariable>();
        private Dictionary<string, double> _inputValues;
        private AndMethod AndMethod;
        private OrMethod OrMethod;

        public OutputCalculator(TakagiSugenoDbContext context)
        {
            _context = context;
        }
        public Dictionary<string, double> CalcOutputsValues(OutputCalcData data) //TODO add or method to db
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            int systemId = data.SystemId;
            _inputValues = data.InputsValues;

            var methods = _context.Systems.Where(s => s.TSSystemId == systemId)
                .Select(s => new { And = s.AndMethod/*, Or = s.OrMethod */}).FirstOrDefault();
            AndMethod = methods.And;
            //OrMethod = methods.Or;

            _inputVariablesWrappers = _context
                .Variables
                .Include(v => v.InputOutput)
                .Where(v => v.InputOutput.TSSystemId == systemId && v.InputOutput.Type == IOType.Input)
                .ToList()
                .Select(v => new InputVariableWrapper(v))
                .ToList();

            _outputVariablesWrappers = _context
                .Variables
                .Where(v => v.InputOutput.TSSystemId == systemId && v.InputOutput.Type == IOType.Output)
                .Select(v => OutputVariableFactory.CreateOutputVariableWrapper(v))
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

            foreach (var output in _context.InputsOutputs.Where(o => o.TSSystemId == systemId && o.Type == IOType.Output))
            {
                res.Add(output.Name, CalcOutputsValue(output));
            }

            return res;
        }

        private double CalcOutputsValue(InputOutput output)
        {
            double temp1 = 0;
            double temp2 = 0;
            foreach(RuleWrapper rule in _ruleWrappers)
            {
                RuleElement elem = rule.Rule.RuleElements.FirstOrDefault(e => e.InputOutputId == output.InputOutputId);
                if (elem != null && elem.VariableId != null)
                {
                    double variableValue = _outputVariablesWrappers.FirstOrDefault(v => v.Variable.VariableId == elem.VariableId).GetValue(_inputValues);
                    temp1 += (rule.CalculatedValue * variableValue);
                    temp2 += rule.CalculatedValue;
                }                
            }
            return temp2 != 0 ? temp1/temp2 : 0;
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

        private double PerfromOrOpeation(double res, double val) //TODO implement
        {
            throw new NotImplementedException();
        }

        public List<MembershipDegree> CalcRuleMembershipDegrees(Rule rule) //TODO not-set variable (done?)
        {
            List<MembershipDegree> degrees = new List<MembershipDegree>();
            foreach (RuleElement elem in rule.RuleElements.Where(e => e.Type == RuleElementType.InputPart))
            {
                InputVariableWrapper variable = _inputVariablesWrappers.FirstOrDefault(v => v.InputId == elem.InputOutputId && v.VariableId == elem.VariableId);
                if(variable != null)
                {
                    double inputValue = _inputValues[variable.InputName];
                    degrees.Add(new MembershipDegree
                    {
                        Value = variable.MembershipFunction.CalcMembership(inputValue),
                        NextOperation = elem.NextOpartion
                    });
                }
            }
            return degrees;
        }

    }

    

    
}
