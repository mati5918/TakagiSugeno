using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.MembershipFunctions
{
    public interface IMembershipFunction
    {
        Dictionary<string, double> FunctionData { get; set; }
        double CalcMembership(double value);
    }
}
