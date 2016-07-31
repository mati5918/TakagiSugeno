using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using System.Linq.Expressions;

namespace TakagiSugeno.Model.Repository
{
    public class RuleElementRepository : IRepository<RuleElement>
    {
        private TakagiSugenoDbContext _context;
        public RuleElementRepository(TakagiSugenoDbContext context)
        {
            _context = context;
        }

        public void Add(RuleElement entity)
        {
            _context.RuleElements.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(RuleElement entity)
        {
            _context.RuleElements.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<RuleElement> Get(Expression<Func<RuleElement, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter cannot be null");
            return _context.RuleElements.Where(filter);
        }

        public IEnumerable<RuleElement> GetAll()
        {
            return _context.RuleElements.ToList();
        }

        public RuleElement GetById(int id)
        {
            return _context.RuleElements.FirstOrDefault(re => re.RuleElementId == id);
        }

        public IEnumerable<RuleElement> GetBySystemId(int systemId)
        {
            return _context.RuleElements.Where(re => re.Rule.TSSystemId == systemId);
        }

        public void Update(RuleElement entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
