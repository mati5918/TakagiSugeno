using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;
using Microsoft.EntityFrameworkCore;


namespace TakagiSugeno.Model.Repository
{
    public class InputOutputRepository : IRepository<InputOutput>
    {
        private TakagiSugenoDbContext _context;
        public InputOutputRepository(TakagiSugenoDbContext context)
        {
            _context = context;
        }
        public void Add(InputOutput entity)
        {
            _context.InputsOutputs.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(InputOutput entity)
        {
            _context.InputsOutputs.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<InputOutput> Get(Expression<Func<InputOutput, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter cannot be null");
            return _context.InputsOutputs.Include(io => io.Variables).Where(filter);
        }

        public IEnumerable<InputOutput> GetAll()
        {
            return _context.InputsOutputs.ToList();
        }

        public InputOutput GetById(int id)
        {
            return _context.InputsOutputs.Include(io => io.Variables)
                .FirstOrDefault(io => io.InputOutputId == id);
        }

        public IEnumerable<InputOutput> GetBySystemId(int systemId)
        {
            return _context.InputsOutputs.Include(io => io.Variables)
                .Where(io => io.TSSystemId == systemId);
        }

        public void Update(InputOutput entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
