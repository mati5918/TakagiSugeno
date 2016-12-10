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
        private List<string> outputPartLog = new List<string>();

        public OutputCalculator(TakagiSugenoDbContext context)
        {
            _context = context;
        }
        public OutputCalcResults CalcOutputsValues(OutputCalcData data)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            int systemId = data.SystemId;
            _inputValues = data.InputsValues;

            var methods = _context.Systems.Where(s => s.TSSystemId == systemId)
                .Select(s => new { And = s.AndMethod, Or = s.OrMethod }).FirstOrDefault();
            AndMethod = methods.And;
            OrMethod = methods.Or;

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
                    .ThenInclude(e => e.Variable)
                    .ThenInclude(e => e.InputOutput)
                .Where(r => r.TSSystemId == systemId)
                .Select(r => new RuleWrapper()
                {
                    Rule = r,
                    MembershipDegrees = CalcRuleMembershipDegrees(r)
                    //CalculatedValue = PerformRuleOperation(this)
                })
                .ToList();

            PerformRulesOperations();
            InitLog();

            foreach (var output in _context.InputsOutputs.Where(o => o.TSSystemId == systemId && o.Type == IOType.Output))
            {
                res.Add(output.Name, Math.Round(CalcOutputsValue(output),3));
            }
        
            string log = CreateLog();
            
            return new OutputCalcResults {CalculatedValues = res, InfoLog = log };
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
                    rule.RuleLog += $"{rule.LogPrefix} * {elem.Variable.Name} = {temp1}{Environment.NewLine}";
                }                
            }
            AppendOutputLog(temp1, temp2, output.Name);
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

        private double PerfromOrOpeation(double res, double val)
        {
            switch(this.OrMethod)
            {
                case OrMethod.Max: return Math.Max(res, val);
                case OrMethod.Sum: return res + val - res * val;
            }
            return res;
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
                    double membership = variable.MembershipFunction.CalcMembership(inputValue);
                    degrees.Add(new MembershipDegree
                    {
                        Value = elem.IsNegation ? 1 - membership : membership,
                        NextOperation = elem.NextOpartion
                    });
                }
            }
            return degrees;
        }

        private void InitLog()
        {
            string values = string.Join(", ", _inputValues.Values);
            List<string> logParts = new List<string>();
            foreach(RuleWrapper wrapper in _ruleWrappers)
            {
                int index = _ruleWrappers.IndexOf(wrapper) + 1;
                string prefix = $"P{index}({values})";
                string membershipInfo = $"{prefix} = {wrapper.RuleMemebershipInfo()}";
                string ruleInfo = wrapper.RuleInfo();
                wrapper.LogPrefix = prefix;
                wrapper.RuleLog = $"{index}. {ruleInfo}{Environment.NewLine}{membershipInfo}{Environment.NewLine}";
            }
        }

        private string CreateLog()
        {
            List<string> logParts = new List<string>();
            logParts.Add("------------------------------------------Reguły------------------------------------------");
            foreach (RuleWrapper wrapper in _ruleWrappers)
            {
                logParts.Add(wrapper.RuleLog);
            }
            logParts.Add("------------------------------------------Wyjścia------------------------------------------");
            logParts.AddRange(outputPartLog);
            return string.Join(Environment.NewLine, logParts);
        }

        private void AppendOutputLog(double sum1, double sum2, string name)
        {
            double value = sum2 != 0 ? sum1 / sum2 : 0;
            outputPartLog.Add($"sum(Pi * ui) = {sum1}{Environment.NewLine}sum(Pi) = {sum2}{Environment.NewLine}{name} = {sum1}/{sum2} = {value}{Environment.NewLine}");
        }

    }

    

    
}
