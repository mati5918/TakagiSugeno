using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class TSSystem
    {
        public int TSSystemId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public AndMethod AndMethod { get; set; }
        public List<TSInputOutput> InputsOutputs { get; set; }
        public List<TSRule> Rules { get; set; }
    }
}
