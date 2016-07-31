using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model.Repository
{
    public class SystemRepository : IRepository<TSSystem>
    {
        private TakagiSugenoDbContext _context;
        public SystemRepository(TakagiSugenoDbContext context)
        {
            _context = context;
        }
        public void Add(TSSystem entity)
        {
            _context.Systems.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TSSystem entity)
        {
            _context.Systems.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<TSSystem> Get(Expression<Func<TSSystem, bool>> filter)
        {
            if(filter == null)
                throw new ArgumentNullException("Filter cannot be null");
            return _context.Systems.Where(filter);
        }

        public IEnumerable<TSSystem> GetAll()
        {
            return _context.Systems.ToList();
        }

        public TSSystem GetById(int id)
        {
            return _context.Systems.FirstOrDefault(s => s.TSSystemId == id);
        }

        public IEnumerable<TSSystem> GetBySystemId(int systemId)
        {
            return new List<TSSystem>() { GetById(systemId) };
        }

        public void Update(TSSystem entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
