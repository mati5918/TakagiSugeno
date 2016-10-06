using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Wrappers
{
    public interface IOutputVariable
    {
        double GetValue(Dictionary<string, double> inputs = null);
        Variable Variable { get; }
    }
}
