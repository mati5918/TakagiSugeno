using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class InputVM
    {
        public int InputId { get; set; }
        public string Name { get; set; }
        public int SystemId { get; set; }
        public List<VariableVM> Variables { get; set; }
    }
}
