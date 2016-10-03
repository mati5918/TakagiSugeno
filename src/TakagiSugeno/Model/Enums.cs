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
        Product,
        Min //TODO
    }
    
    public enum OrMethod
    {
        Default //TODO
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
