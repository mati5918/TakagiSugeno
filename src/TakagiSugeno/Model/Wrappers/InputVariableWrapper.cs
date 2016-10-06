using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.MembershipFunctions;

namespace TakagiSugeno.Model.Wrappers
{
    public class InputVariableWrapper
    {
        public InputVariableWrapper(Variable variable)
        {
            _variable = variable; //TODO: get from repo with InputOutput include
            MembershipFunction = MembershipFunctionFactory.CreateMembershipFunction(_variable.Type, _variable.Data);
        }

        private Variable _variable;
        public IMembershipFunction MembershipFunction { get; }
        //public string InputName { get { return _variable.InputOutput.Name; } }
        //public string VariableName { get { return _variable.Name; } }
        public int VariableId { get { return _variable.VariableId; } }
        public int InputId { get { return _variable.InputOutputId; } }
    }
}
