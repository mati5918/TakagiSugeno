using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.MembershipFunctions
{
    public class GaussianMembershipFunction : IMembershipFunction
    {
        public Dictionary<string, double> FunctionData { get; set; }

        public GaussianMembershipFunction(string jsonData)
        {
            FunctionData = JsonConvert.DeserializeObject<Dictionary<string, double>>(jsonData);
        }

        public double CalcMembership(double value)
        {
            double sigma, c;
            if (FunctionData.TryGetValue("sigma", out sigma) && FunctionData.TryGetValue("c", out c))
            {
                double power = (-1 * Math.Pow(value - c, 2)) / (2 * Math.Pow(sigma, 2));
                return Math.Pow(Math.E, power);
            }
            else
            {
                throw new Exception("Invalid FunctionData");
            }
        }
    }
}
