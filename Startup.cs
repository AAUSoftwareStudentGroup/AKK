using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AKK.Classes.Models;
using AKK.Classes.Models.Repository;

namespace AKK
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRepository<Route>,RouteRepository>();
            services.AddSingleton<IRepository<Section>, SectionRepository>();
            services.AddSingleton<IRepository<Grade>, GradeRepository>();

            services.AddMvc();

            var connection = Configuration["ConnectionStrings:DefaultConnection"];

	        services.AddDbContext<MainDbContext>(options =>
	        	options.UseSqlite(connection)
	        );
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            app.UseStaticFiles();

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<MainDbContext>();
                if (db.Database.EnsureCreated())
                {
                    db.Seed();
                }
            }
        }
    }
}
