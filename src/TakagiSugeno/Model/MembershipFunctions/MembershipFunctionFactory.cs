using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.MembershipFunctions
{
    public class MembershipFunctionFactory
    {
        public static IMembershipFunction CreateMembershipFunction(VariableType type, string jsonData)
        {
            switch(type)
            {
                case VariableType.Triangle: return new TriangularMembershipFunction(jsonData);
                case VariableType.Trapeze: return new TrapezoidalMembershipFunction(jsonData);
            }
            return null;
        }
    }
}
