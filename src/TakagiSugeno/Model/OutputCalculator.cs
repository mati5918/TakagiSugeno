using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.ViewModels;
using TakagiSugeno.Model.Wrappers;

namespace TakagiSugeno.Model
{
    public class OutputCalculator
    {      
        private TakagiSugenoDbContext _context;
        //private List<InputOutput> _outputs;
        private IRepository<InputOutput> _ioRepository;
        private IRepository<Rule> _rulesReposistory;
        private IRepository<Variable> _variablesRepository;
        private IRepository<TSSystem> _systemsRepostiory;
        private List<RuleWrapper> _ruleWrappers = new List<RuleWrapper>();
        private List<InputVariableWrapper> _inputVariablesWrappers = new List<InputVariableWrapper>();
        private List<OutputVariableWrapper> _outputVariablesWrappers = new List<OutputVariableWrapper>();
        private Dictionary<string, double> _inputValues;
        private AndMethod andMethod;
        private OrMethod orMethod;
        private List<string> outputPartLog = new List<string>();

        public OutputCalculator(/*TakagiSugenoDbContext context,*/ IRepository<InputOutput> ioRepository, IRepository<Rule> rulesReposistory, 
            IRepository<Variable> variablesRepository, IRepository<TSSystem> systemsRepository)
        {
            //_context = context;
            _ioRepository = ioRepository;
            _rulesReposistory = rulesReposistory;
            _variablesRepository = variablesRepository;
            _systemsRepostiory = systemsRepository;
        }
        public OutputCalcResults CalcOutputsValues(OutputCalcData data)
        {
            int systemId = data.SystemId;
            _inputValues = data.InputsValues;

            ReadAndOrMethods(systemId);
            ReadInputVariables(systemId);
            ReadOutputVariables(systemId);
            ReadRules(systemId);

            PerformRulesOperations();
            InitLog();

            Dictionary<int, double> outputsValues = CalcOutputsValues(systemId);
            string log = CreateLog();

            return new OutputCalcResults {CalculatedValues = outputsValues,
                InfoLog = log };
        }

        private Dictionary<int, double> CalcOutputsValues(int systemId)
        {
            Dictionary<int, double> outputsValues = new Dictionary<int, double>();
            foreach (var output in _ioRepository.GetBySystemId(systemId).Where(o => o.Type == IOType.Output))
            {
                outputsValues.Add(output.InputOutputId, Math.Round(CalcOutputValue(output), 3));
            }
            return outputsValues;
        }

        private double CalcOutputValue(InputOutput output)
        {
            double nominator = 0;
            double denominator = 0;
            foreach(RuleWrapper rule in _ruleWrappers)
            {
                RuleElement elem = rule.Rule.RuleElements
                    .FirstOrDefault(e => e.InputOutputId == output.InputOutputId);
                if (elem != null && elem.VariableId != null)
                {
                    double variableValue = _outputVariablesWrappers
                        .FirstOrDefault(v => v.Variable.VariableId == elem.VariableId)
                        .Function.GetValue(_inputValues);
                    nominator += (rule.CalculatedValue * variableValue);
                    denominator += rule.CalculatedValue;
                    rule.RuleLog += $"{rule.LogPrefix} * {elem.Variable.Name} = {Math.Round(nominator, 3)}{Environment.NewLine}";
                }                
            }
            AppendOutputLog(nominator, denominator, output.Name);
            return denominator != 0 ? nominator/denominator : 0;
        }

        private void PerformRulesOperations()
        {
            foreach(RuleWrapper val in _ruleWrappers)
            {
                val.CalculatedValue = PerformRuleOperations(val);
            }
        }

        private double PerformRuleOperations(RuleWrapper rule)
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
            switch(this.andMethod)
            {
                case AndMethod.Product: return res * val;
                case AndMethod.Min: return Math.Min(res, val);
            }
            return res;
        }

        private double PerfromOrOpeation(double res, double val)
        {
            switch(this.orMethod)
            {
                case OrMethod.Max: return Math.Max(res, val);
                case OrMethod.Sum: return res + val - res * val;
            }
            return res;
        }

        private List<MembershipDegree> CalcRuleMembershipDegrees(Rule rule)
        {
            List<MembershipDegree> degrees = new List<MembershipDegree>();
            foreach (RuleElement elem in rule.RuleElements.Where(e => e.Type == RuleElementType.InputPart).OrderBy(r => r.RuleElementId))
            {
                InputVariableWrapper variable = _inputVariablesWrappers.FirstOrDefault(v => v.InputId == elem.InputOutputId && v.VariableId == elem.VariableId);
                if (variable != null)
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


        private void ReadAndOrMethods(int systemId)
        {
            var methods = _systemsRepostiory.GetBySystemId(systemId)
                .Select(s => new { And = s.AndMethod, Or = s.OrMethod }).FirstOrDefault();
            andMethod = methods.And;
            orMethod = methods.Or;
        }

        private void ReadInputVariables(int systemId)
        {
            _inputVariablesWrappers = _variablesRepository
                .Get(v => v.InputOutput.TSSystemId == systemId 
                        && v.InputOutput.Type == IOType.Input)
                .Select(v => new InputVariableWrapper(v))
                .ToList();
        }

        private void ReadOutputVariables(int systemId)
        {
            _outputVariablesWrappers = _variablesRepository
               .Get(v => v.InputOutput.TSSystemId == systemId && v.InputOutput.Type == IOType.Output)
               .Select(v => new OutputVariableWrapper(v))
               .ToList();
        }

        private void ReadRules(int systemId)
        {
            _ruleWrappers = _rulesReposistory
                .GetBySystemId(systemId)
                .Select(r => new RuleWrapper()
                {
                    Rule = r,
                    MembershipDegrees = CalcRuleMembershipDegrees(r)
                }).ToList();
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
            sum1 = Math.Round(sum1, 3);
            sum2 = Math.Round(sum2, 3);
            value = Math.Round(value, 3);
            outputPartLog.Add($"{name}:{Environment.NewLine}sum(Pi * ui) = {sum1}{Environment.NewLine}sum(Pi) = {sum2}{Environment.NewLine}{name} = {sum1}/{sum2} = {value}{Environment.NewLine}");
        }

    }

    

    
}
