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
            _variable = variable;
            MembershipFunction = MembershipFunctionFactory.CreateMembershipFunction(_variable.Type, _variable.Data);
        }

        private Variable _variable;
        public IMembershipFunction MembershipFunction { get; }
        public int VariableId { get { return _variable.VariableId; } }
        public string InputName { get { return _variable.InputOutput.Name; } }
        public int InputId { get { return _variable.InputOutputId; } }
    }
}
