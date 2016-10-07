using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public static class OutputVariableFactory
    {
        public static IOutputVariable CreateOutputVariableWrapper(Variable variable)
        {
            switch(variable.Type)
            {
                case VariableType.OutputConst: return new OutputConstVariable(variable);
                case VariableType.OutputFunction: return new OutputFunctionVariable(variable);
            }
            return null;
        }
    }
}
