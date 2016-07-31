using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using System.Linq.Expressions;

namespace TakagiSugeno.Model.Repository 
{
    public class RuleRepository : IRepository<Rule>
    {
        private TakagiSugenoDbContext _context;
        public RuleRepository(TakagiSugenoDbContext context)
        {
            _context = context;
        }
        public void Add(Rule entity)
        {
            _context.Rules.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Rule entity)
        {
            _context.Rules.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Rule> Get(Expression<Func<Rule, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter cannot be null");
            return _context.Rules.Where(filter);
        }

        public IEnumerable<Rule> GetAll()
        {
            return _context.Rules.ToList();
        }

        public Rule GetById(int id)
        {
            return _context.Rules.Include(r => r.RuleElements).FirstOrDefault(r => r.RuleId == id);
        }

        public IEnumerable<Rule> GetBySystemId(int systemId)
        {
            return _context.Rules.Include(r => r.RuleElements).Where(r => r.TSSystemId == systemId);
        }

        public void Update(Rule entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
