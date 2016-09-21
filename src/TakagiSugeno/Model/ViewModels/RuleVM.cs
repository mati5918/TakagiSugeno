using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class RuleGeneralVM
    {
        public List<RuleVM> Rules { get; set; }
        public Dictionary<int, IEnumerable<SelectListItem>> VariablesLists { get; set; }
        public Dictionary<int, Dictionary<int,string>> ChartsData { get; set; }
    }
    public class RuleVM
    {
        public List<RuleElementVM> RuleElements { get; set; }
        public int RuleId { get; set; }
        public int SystemId { get; set; }
    }

    public class RuleElementVM
    {
        public int ElementId { get; set; }
        public RuleElementType Type { get; set; }
        public int VariableId { get; set; }
        public int InputOutputId { get; set; }
        public string VariableName { get; set; }
        public string InputOutputName { get; set; }
    }
}
