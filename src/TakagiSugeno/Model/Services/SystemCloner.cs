using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Services
{
    public class SystemCloner
    {
        private TakagiSugenoDbContext _context;

        public SystemCloner(TakagiSugenoDbContext context)
        {
            _context = context;
        }

        public string SystemToJson(int systemId)
        {
            TSSystem system = ReadFullSystem(systemId);

            foreach (InputOutput io in system.InputsOutputs)
            {
                io.System = null;
                foreach (Variable var in io.Variables)
                {
                    var.InputOutput = null;
                }
            }
            foreach (Rule r in system.Rules)
            {
                r.System = null;
                foreach (RuleElement elem in r.RuleElements)
                {
                    elem.Rule = null;
                    elem.InputOutput = null;
                    elem.Variable = null;
                }
            }
            return JsonConvert.SerializeObject(system, Formatting.Indented);
        }

        public int? ReadFromFile(IFormFile file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                {
                    string systemJson = reader.ReadToEnd();
                    TSSystem newSystem = JsonToSystem(systemJson);
                    return CopySystem(newSystem);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private TSSystem JsonToSystem(string systemJson)
        {
            TSSystem system = JsonConvert.DeserializeObject<TSSystem>(systemJson);
            return system;
        }

        private TSSystem ReadFullSystem(int systemId)
        {
            return _context.Systems.AsNoTracking()
                .Include(s => s.InputsOutputs)
                    .ThenInclude(io => io.Variables)
                .Include(s => s.Rules)
                    .ThenInclude(r => r.RuleElements)
                .FirstOrDefault(s => s.TSSystemId == systemId);
        }

        public int CloneSystem(int systemId)
        {
            TSSystem sourceSystem = ReadFullSystem(systemId);
            return CopySystem(sourceSystem);
        }

        private int CopySystem(TSSystem sourceSystem)
        {
            TSSystem newSystem = new TSSystem
            {
                AndMethod = sourceSystem.AndMethod,
                OrMethod = sourceSystem.OrMethod,
                CreatedDate = DateTime.Now,
                IsPublished = false
            };

            _context.Systems.Add(newSystem);
            _context.SaveChanges();

            Dictionary<int, int> ioMap = new Dictionary<int, int>();
            Dictionary<int, int> variablesMap = new Dictionary<int, int>();
            foreach (InputOutput io in sourceSystem.InputsOutputs)
            {
                InputOutput newIo = new InputOutput
                {
                    Name = io.Name,
                    Type = io.Type,
                    TSSystemId = newSystem.TSSystemId,
                    RangeStart = io.RangeStart,
                    RangeEnd = io.RangeEnd
                };
                _context.InputsOutputs.Add(newIo);
                _context.SaveChanges();

                ioMap.Add(io.InputOutputId, newIo.InputOutputId);

                foreach (Variable var in io.Variables)
                {
                    Variable newVar = new Variable
                    {
                        Data = var.Data,
                        Name = var.Name,
                        Type = var.Type,
                        InputOutputId = newIo.InputOutputId
                    };
                    _context.Variables.Add(newVar);
                    _context.SaveChanges();
                    variablesMap.Add(var.VariableId, newVar.VariableId);
                }

            }

            foreach (Rule r in sourceSystem.Rules)
            {
                Rule newRule = new Rule
                {
                    TSSystemId = newSystem.TSSystemId
                };
                _context.Rules.Add(newRule);
                _context.SaveChanges();

                foreach (RuleElement elem in r.RuleElements)
                {
                    int? fakeId = null;
                    RuleElement newElem = new RuleElement
                    {
                        IsNegation = elem.IsNegation,
                        Type = elem.Type,
                        NextOpartion = elem.NextOpartion,
                        RuleId = newRule.RuleId,
                        VariableId = elem.VariableId.HasValue ? variablesMap[elem.VariableId.Value] : fakeId,
                        InputOutputId = ioMap[elem.InputOutputId]
                    };
                    _context.RuleElements.Add(newElem);
                }
                _context.SaveChanges();

            }

            return newSystem.TSSystemId;
        }
    }
}
