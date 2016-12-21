using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public class OutputVariableWrapper
    {
        public OutputVariableWrapper(Variable variable)
        {
            Variable = variable;
            Function = OutputVariableFactory.CreateOutputVariableFunction(variable);
        }

        public IOutputVariableFunction Function { get; }
        public Variable Variable { get; }
    }
}
