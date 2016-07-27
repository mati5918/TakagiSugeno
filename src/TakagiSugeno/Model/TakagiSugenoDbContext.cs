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
        public TakagiSugenoDbContext(DbContextOptions<TakagiSugenoDbContext> options) : base(options)
        {
        }

        public DbSet<TSSystem> Systems { get; set; }
        public DbSet<TSInputOutput> InputsOutputs { get; set; }
        public DbSet<TSRule> Rules { get; set; }
        public DbSet<TSRuleElement> RuleElements { get; set; }
        public DbSet<TSVariable> Variables { get; set; }

    }
}
