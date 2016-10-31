using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.Services
{
    public class SystemStateHelper
    {
        private TakagiSugenoDbContext _context;

        public SystemStateHelper(TakagiSugenoDbContext context)
        {
            _context = context;
        }

        public int GetInputsCount(int systemId)
        {
            return _context.InputsOutputs.Count(i => i.Type == IOType.Input && i.TSSystemId == systemId);
        }

        public int GetOutputsCount(int systemId)
        {
            return _context.InputsOutputs.Count(o => o.Type == IOType.Output && o.TSSystemId == systemId);
        }

        public int GetRulesCount(int systemId)
        {
            return _context.Rules.Count(r => r.TSSystemId == systemId);
        }

        public bool IsSystemPublished(int systemId)
        {
            return _context.Systems.FirstOrDefault(s => s.TSSystemId == systemId)?.IsPublished ?? false;
        }
    }
}
