using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.MembershipFunctions
{
    public class TriangularMembershipFunction : IMembershipFunction
    {
        public Dictionary<string, double> FunctionData { get; set; }

        public TriangularMembershipFunction(string jsonData)
        {
            FunctionData = JsonConvert.DeserializeObject<Dictionary<string, double>>(jsonData);
        }

        public double CalcMembership(double value)
        {
            double a, b, c;
            if(FunctionData.TryGetValue("a", out a) && FunctionData.TryGetValue("b",out b) && FunctionData.TryGetValue("c",out c))
            {
                if ((a < value) && (value <= b))
                    return (value - a) / (b - a);
                if ((b < value) && (value < c))
                    return (c - value) / (c - b);
                return 0;
            }
            else
            {
                throw new Exception("Invalid FunctionData");
            }

        }
    }
}
