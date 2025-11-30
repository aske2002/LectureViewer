using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using backend.Domain.Entities;

namespace backend.Application.Transcriptions.EventsHandlers;

public class TranscriptionCompletedEventHandler : INotificationHandler<JobSuccessEvent<TranscriptionMediaProcessingJob>>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ILogger<TranscriptionCompletedEventHandler> _logger;

    public TranscriptionCompletedEventHandler(
        IBackgroundTaskQueue backgroundTaskQueue,
        ILogger<TranscriptionCompletedEventHandler> logger)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
    }

    public Task Handle(JobSuccessEvent<TranscriptionMediaProcessingJob> notification, CancellationToken cancellationToken)
    {
        var transcript = notification.Job.Transcript;

        if (transcript == null)
        {
            _logger.LogWarning("TranscriptionCompletedEventHandler: Transcript is null for job {JobId}", notification.Job.Id);
            return Task.CompletedTask;
        }

        _backgroundTaskQueue.QueueBackgroundWorkItem(async (sp, token) =>
        {
            var mediaJobService = sp.GetRequiredService<IMediaJobService>();
            await mediaJobService.CreateJob(new ResumeExtractionMediaProcessingJob()
            {
                ParentJobId = notification.Job.Id,
                TargetLineLength = 20,
                SourceText = transcript.TranscriptText,
            }, cancellationToken);

            await mediaJobService.CreateJob(new KeywordExtractionMediaProcessingJob()
            {
                ParentJobId = notification.Job.Id,
                SourceText = transcript.TranscriptText,
            }, cancellationToken);
        });

        return Task.CompletedTask;
    }
}
