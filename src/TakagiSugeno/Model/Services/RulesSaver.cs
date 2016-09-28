using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Services
{
    public class RulesSaver
    {
        private TakagiSugenoDbContext _context;
        public RulesSaver(TakagiSugenoDbContext context)
        {
            _context = context;
        }

        public void Save(List <RuleVM> rules)
        {
            IEnumerable<RuleVM> existedRules = rules.Where(r => r.RuleId >= 0);
            IEnumerable<RuleVM> newRules = rules.Where(r => r.RuleId < 0);
            IEnumerable<Rule> systemRules = _context.Rules.Include(r => r.RuleElements).Where(r => r.TSSystemId == rules.FirstOrDefault().SystemId);

            foreach(var rule in systemRules)
            {
                RuleVM itemToUpdate = existedRules.FirstOrDefault(r => r.RuleId == rule.RuleId);
                if(itemToUpdate != null) //rule exists
                {
                    ModifyRuleElements(rule, itemToUpdate);
                    _context.Entry(rule).State = EntityState.Modified;
                }
                else //remove rule
                {
                    _context.Rules.Remove(rule);
                }
            }
            _context.SaveChanges();
            foreach(var rule in newRules)
            {
                Rule entity = new Rule { TSSystemId = rule.SystemId };
                _context.Rules.Add(entity);
                _context.SaveChanges();
                CreateRuleElements(entity, rule);
            }
            //_context.SaveChanges();
        }

        private void CreateRuleElements(Rule entity, RuleVM vm)
        {
            foreach (var elem in vm.RuleElements)
            {
                RuleElement elemEntity = new RuleElement
                {
                    InputOutputId = elem.InputOutputId,
                    IsNegation = elem.IsNegation,
                    NextOpartion = elem.NextOperation,
                    RuleId = entity.RuleId,
                    Type = elem.Type,
                };
                if (elem.VariableId == -1)
                {
                    elemEntity.VariableId = null;
                }
                else
                {
                    elemEntity.VariableId = elem.VariableId;
                }
                _context.RuleElements.Add(elemEntity);
                _context.SaveChanges();
            }
        }

        private void ModifyRuleElements(Rule entity, RuleVM vm)
        {
            foreach(var elem in entity.RuleElements)
            {
                RuleElementVM itemToUpdate = vm.RuleElements.FirstOrDefault(e => e.ElementId == elem.RuleElementId);
                if(itemToUpdate == null)
                {
                    throw new Exception("Error in modify rule elements");
                }
                elem.IsNegation = itemToUpdate.IsNegation;
                elem.NextOpartion = itemToUpdate.NextOperation;
                if(itemToUpdate.VariableId == -1)
                {
                    elem.VariableId = null;
                }
                else
                {
                    elem.VariableId = itemToUpdate.VariableId;
                }
                _context.Entry(elem).State = EntityState.Modified;
            }
        }
    }
}
