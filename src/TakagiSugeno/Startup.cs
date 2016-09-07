using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TakagiSugeno.Model;
using Microsoft.EntityFrameworkCore;
using TakagiSugeno.Model.Repository;
using TakagiSugeno.Model.Entity;
using TakagiSugeno.Model.Services;

namespace TakagiSugeno
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDbContext<TakagiSugenoDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<TakagiSugenoDbContext>();
            services.AddScoped<InputsService>();
            services.AddScoped<ChartsService>();
            services.AddScoped<VariablesService>();
            services.AddScoped<InputSaver>();
            RegisterRepositories(services);
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepository<TSSystem>, SystemRepository>();
            services.AddScoped<IRepository<Variable>, VariableRepository>();
            services.AddScoped<IRepository<InputOutput>, InputOutputRepository>();
            services.AddScoped<IRepository<Rule>, RuleRepository>();
            services.AddScoped<IRepository<RuleElement>, RuleElementRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=SystemOverview}/{action=Index}/{id?}");
            });

            SeedData.Initialize(app.ApplicationServices);
        }
    }
}
