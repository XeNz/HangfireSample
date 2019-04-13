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
            EnqueueStartupJobs();
            EnqueueRecurringJobs();

            return Task.CompletedTask;
        }

        private void EnqueueStartupJobs()
        {
            try
            {
                CreateommentHistoryViewCountUpdateForStartup();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while enqueueing startup job", e);
            }
        }

        private void EnqueueRecurringJobs()
        {
            try
            {
                CreateFiveMinutelyCommentHistoryViewCountUpdate();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while enqueueing recurring job", e);
            }
        }

        private void CreateommentHistoryViewCountUpdateForStartup()
        {
            using (var commentService = _scope.ServiceProvider.GetRequiredService<ICommentService>())
            {
                // every 5 minutes
                BackgroundJob.Enqueue(() => commentService.UpdateHistoryCount());
            }
        }

        private void CreateFiveMinutelyCommentHistoryViewCountUpdate()
        {
            using (var commentService = _scope.ServiceProvider.GetRequiredService<ICommentService>())
            {
                // every 5 minutes
                RecurringJob.AddOrUpdate(() => commentService.UpdateHistoryCount(), "*/5 * * * *");
            }
        }
    }
}