using System;
using Hangfire;
using Hangfire.PostgreSql;
using HangfireSample.Business.Services;
using HangfireSample.Business.Services.Interfaces;
using HangfireSample.DataProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace HangfireSample
{
    public static class DependencyRegistration
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.TryAddScoped<EntityContext>();

            services.TryAddScoped<ICommentService, CommentService>();
        }

        public static void AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient("CommentApi", c =>
            {
                c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        public static void ConfigureHangfireServices(this IServiceCollection services, string connectionString)
        {
            GlobalConfiguration.Configuration.UsePostgreSqlStorage(connectionString);
            var storage = JobStorage.Current;
            services.AddHangfire(options => options.UseStorage(storage));
        }
    }
}