using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakagiSugeno.Model.Entity;

namespace TakagiSugeno.Model
{
    public class TakagiSugenoDbContext : DbContext
    {
        public TakagiSugenoDbContext(DbContextOptions<TakagiSugenoDbContext> options) 
            : base(options)
        {
        }

        public DbSet<TSSystem> Systems { get; set; }
        public DbSet<InputOutput> InputsOutputs { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<RuleElement> RuleElements { get; set; }
        public DbSet<Variable> Variables { get; set; }

    }
}
