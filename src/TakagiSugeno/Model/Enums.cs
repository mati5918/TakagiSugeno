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
        Trapeze //TODO: add more

    }

    public enum IOType
    {
        Input,
        Output
    }

    public enum AndMethod
    {
        Default //TODO
    }
    
    public enum OrMethod
    {
        Default //TODO
    }

    public enum RuleElementType
    {
        Default //TODO
    }
}
