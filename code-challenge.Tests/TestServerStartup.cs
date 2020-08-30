using challenge.Data;
using challenge.Repositories;
using challenge.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace code_challenge.Tests.Integration
{
    public class TestServerStartup
    {
        public TestServerStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase("EmployeeDB");
                options.UseInMemoryDatabase("CompensationDB");
            });
            services.AddScoped<IEmployeeContext, EmployeeContext>();
            services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();
            services.AddTransient<EmployeeDataSeeder>();
            services.AddTransient<CompensationDataSeeder>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IReportingStructureService, ReportingStructureService>();
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EmployeeDataSeeder seeder, CompensationDataSeeder compensationSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.Seed().Wait();
                compensationSeeder.Seed().Wait();
            }
            
            app.UseMvc();

        }
    }
}
