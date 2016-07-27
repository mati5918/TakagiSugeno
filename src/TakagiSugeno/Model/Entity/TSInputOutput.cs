using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Entity
{
    public class TSInputOutput
    {
        public int TSInputOutputId { get; set; }
        public int Name { get; set; }
        public IOType Type { get; set; }

        public List<TSVariable> Variables { get; set; }
        public int TSSystemId { get; set; }
        public TSSystem System { get; set; }
    }
}
