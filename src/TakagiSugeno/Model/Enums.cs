using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model
{
    public enum VariableType
    {
        [Display(Name = "Trójkąt")]
        Triangle,
        [Display(Name = "Trapez")]
        Trapeze,
        [Display(Name = "Gaussian")]
        Gaussian,
        [Display(Name = "Stała")]
        OutputConst = 100,
        [Display(Name = "Funkcja")]
        OutputFunction = 101
    }

    public enum IOType
    {
        Input,
        Output
    }

    public enum AndMethod
    {
        [Display(Name = "Iloczyn algebraiczny")]
        Product,
        [Display(Name = "Minimum")]
        Min 
    }
    
    public enum OrMethod
    {
        [Display(Name = "Suma algebraiczna")]
        Sum,
        [Display(Name = "Maksimum")]
        Max //TODO
    }

    public enum RuleElementType
    {
        InputPart,
        OutputPart
    }

    public enum RuleNextOperation
    {
        And = 0,
        Or = 1,
        None = 2
    }
}
