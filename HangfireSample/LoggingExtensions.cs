using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace HangfireSample
{
    public static class LogExtensions
    {
        public static void ConfigureLogging(this IApplicationBuilder app, IConfiguration configuration)
        {
#if DEBUG
            Serilog.Debugging.SelfLog.Enable(Console.Error);
#endif
            var logConfiguration = new LoggerConfiguration()
                .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(configuration.GetSection("DEVELOPMENTLOG")))
                .MinimumLevel.Information()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .Enrich.With(app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray())
                .Enrich.FromLogContext();
            Log.Logger = logConfiguration.CreateLogger();
        }
    }
}