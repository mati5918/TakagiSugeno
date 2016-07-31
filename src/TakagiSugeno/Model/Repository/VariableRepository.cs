using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Repository
{
    public class VariableRepository : IRepository<Variable>
    {
        private TakagiSugenoDbContext _context;
        public VariableRepository(TakagiSugenoDbContext context)
        {
            _context = context;       
        }
        public void Add(Variable entity)
        {
            _context.Variables.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Variable entity)
        {
            _context.Variables.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Variable> Get(Expression<Func<Variable, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter cannot be null");
            return _context.Variables.Where(filter);
        }

        public IEnumerable<Variable> GetAll()
        {
            return _context.Variables.ToList();
        }

        public Variable GetById(int id)
        {
            return _context.Variables.FirstOrDefault(v => v.VariableId == id);
        }

        public IEnumerable<Variable> GetBySystemId(int systemId)
        {
            return _context.Variables.Where(v => v.InputOutput.TSSystemId == systemId);
        }

        public void Update(Variable entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
