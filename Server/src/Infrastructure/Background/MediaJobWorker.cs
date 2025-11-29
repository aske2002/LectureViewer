using backend.Domain.Entities;
using backend.Domain.Events;
using backend.Infrastructure.MediaProcessing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class MediaJobWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MediaJobWorker> _logger;

    public MediaJobWorker(IServiceScopeFactory scopeFactory, ILogger<MediaJobWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var _jobService = scope.ServiceProvider.GetRequiredService<IMediaJobService>();
            var result = await _jobService.TryAcquireNextPendingJobAsync(stoppingToken);

            if (result is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                continue;
            }

            var (job, attempt) = result.Value;
            var genericHandlerType = typeof(IMediaJobHandler<>).MakeGenericType(job.GetType());
            var handler = scope.ServiceProvider.GetService(genericHandlerType) as IMediaJobHandler;

            if (handler is null)
            {
                _logger.LogError("No handler found for job type {JobType} (Job ID: {JobId})", job.JobType, job.Id);
                await _jobService.MarkAttemptFailedAsync(attempt.Id, "No handler found for job type.", stoppingToken);
                continue;
            }
            try
            {
                _logger.LogInformation("Executing {JobType} job {JobId}", job.JobType, job.Id);
                await handler.HandleAsync(job, attempt, stoppingToken);

                await _jobService.MarkAttemptCompleted(attempt.Id, stoppingToken);
                _logger.LogInformation("Completed {JobType} job {JobId}", job.JobType, job.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing {JobType} job {JobId}", job.JobType, job.Id);
                await _jobService.MarkAttemptFailedAsync(attempt.Id, ex.Message, stoppingToken);
            }
        }
    }
}
