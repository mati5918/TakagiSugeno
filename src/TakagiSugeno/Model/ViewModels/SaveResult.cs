using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class SaveResult
    {
        public int Id { get; set; }
        public List<string> Errors { get; set; }
    }
}
