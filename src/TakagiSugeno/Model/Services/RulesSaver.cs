using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.ViewModels;

namespace TakagiSugeno.Model.Services
{
    public class RulesSaver
    {
        private TakagiSugenoDbContext _context;
        private IRepository<Rule> _rulesRepository;
        private IRepository<RuleElement> _ruleElementsRepository;
        public RulesSaver(TakagiSugenoDbContext context, IRepository<Rule> rulesRepository, IRepository<RuleElement> ruleElementsRepository)
        {
            _context = context;
            _rulesRepository = rulesRepository;
            _ruleElementsRepository = ruleElementsRepository;
        }

        public void Save(List <RuleVM> rules)
        {
            IEnumerable<RuleVM> existedRules = rules.Where(r => r.RuleId >= 0);
            IEnumerable<RuleVM> newRules = rules.Where(r => r.RuleId < 0);
            IEnumerable<Rule> systemRules = _rulesRepository
                .GetBySystemId(rules.FirstOrDefault().SystemId).ToList();
            //IEnumerable<Rule> systemRules = _context.Rules.Include(r => r.RuleElements).Where(r => r.TSSystemId == rules.FirstOrDefault().SystemId);

            foreach (var rule in systemRules)
            {
                RuleVM itemToUpdate = existedRules
                    .FirstOrDefault(r => r.RuleId == rule.RuleId);
                if(itemToUpdate != null) //rule exists
                {
                    ModifyRuleElements(rule, itemToUpdate);
                    _rulesRepository.Update(rule);
                    //_context.Entry(rule).State = EntityState.Modified;
                }
                else //remove rule
                {
                    _rulesRepository.Delete(rule);
                    //_context.Rules.Remove(rule);
                }
            }
            //_context.SaveChanges();
            foreach(var rule in newRules)
            {
                Rule entity = new Rule { TSSystemId = rule.SystemId };
                _rulesRepository.Add(entity);
                //_context.Rules.Add(entity);
                //_context.SaveChanges();
                CreateRuleElements(entity, rule);
            }
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
                _ruleElementsRepository.Add(elemEntity);
                //_context.RuleElements.Add(elemEntity);
                //_context.SaveChanges();
            }
        }

        private void ModifyRuleElements(Rule entity, RuleVM vm)
        {
            foreach(var elem in entity.RuleElements.ToList())
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
                _ruleElementsRepository.Update(elem);
                //_context.Entry(elem).State = EntityState.Modified;
            }
        }
    }
}
