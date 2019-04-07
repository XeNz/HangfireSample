using Hangfire;
using HangfireSample.Business.Services;
using HangfireSample.DataProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HangfireSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("defaultConnection");

            services.AddMvc(options => { options.EnableEndpointRouting = true; })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<EntityContext>(options => { options.UseNpgsql(connString); });
            services.AddBusinessServices();
            services.AddHttpClients();
            services.ConfigureHangfireServices(connString);
            services.AddHostedService<RecurringJobsService>();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.ConfigureLogging(Configuration);

            app.UseMvc();


            app.UseHangfireServer(new BackgroundJobServerOptions {WorkerCount = 1});
            app.UseHangfireDashboard();
        }
    }
}