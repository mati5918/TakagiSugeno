using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.MembershipFunctions
{
    public class TrapezoidalMembershipFunction : IMembershipFunction
    {
        public Dictionary<string, double> FunctionData { get; set; }

        public TrapezoidalMembershipFunction(string jsonData)
        {
            FunctionData = JsonConvert.DeserializeObject<Dictionary<string, double>>(jsonData);
        }

        public double CalcMembership(double value)
        {
            double a, b, c, d;
            if (FunctionData.TryGetValue("a", out a) && FunctionData.TryGetValue("b", out b) && FunctionData.TryGetValue("c", out c) && FunctionData.TryGetValue("d", out d))
            {
                if ((a < value) && (value <= b))
                    return (value - a) / (b - a);
                if ((b < value) && (value <= c))
                    return 1;
                if ((c < value) && (value < d))
                    return (d - value) / (d - c);
                return 0;
            }
            else
            {
                throw new Exception("Invalid FunctionData");
            }
        }
    }
}
