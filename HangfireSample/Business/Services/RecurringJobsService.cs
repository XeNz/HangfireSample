using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using HangfireSample.Business.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HangfireSample.Business.Services
{
    public class RecurringJobsService : BackgroundService
    {
        private readonly ILogger<RecurringJobsService> _logger;
        private readonly IServiceScope _scope;

        public RecurringJobsService(ILogger<RecurringJobsService> logger, IServiceProvider services)
        {
            _scope = services.CreateScope();
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                CreateFiveMinutelyCommentHistoryViewCount();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while enqueueing recurring job", e);
            }

            return Task.CompletedTask;
        }

        private void CreateFiveMinutelyCommentHistoryViewCount()
        {
            using (var commentService = _scope.ServiceProvider.GetRequiredService<ICommentService>())
            {
                // every 5 minutes
                RecurringJob.AddOrUpdate(() => commentService.UpdateHistoryCount(), "*/5 * * * *");
            }
        }
    }
}