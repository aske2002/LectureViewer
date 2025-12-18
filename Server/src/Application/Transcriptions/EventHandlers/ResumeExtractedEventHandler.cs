using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application.Transcriptions.EventsHandlers;

public class ResumeExtractedEventHandler : INotificationHandler<JobSuccessEvent<ResumeExtractionMediaProcessingJob>>
{
    private readonly ILogger<ResumeExtractedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public ResumeExtractedEventHandler(
        ILogger<ResumeExtractedEventHandler> logger,
        IApplicationDbContext dbContext,
        IBackgroundTaskQueue backgroundTaskQueue)
    {
        _logger = logger;
        _dbContext = dbContext;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public async Task Handle(JobSuccessEvent<ResumeExtractionMediaProcessingJob> notification, CancellationToken cancellationToken)
    {
        var resume = notification.Job;
        var transcriptionJob = await notification.Job.GetJobOfType<TranscriptionMediaProcessingJob>(_dbContext, cancellationToken);

        if (transcriptionJob == null)
        {
            return;
        }

        await _dbContext.Entry(transcriptionJob).Reference(j => j.Transcript).LoadAsync(cancellationToken);
        var transcript = transcriptionJob?.Transcript;

        if (transcript == null)
        {
            return;
        }

        transcript.Summary = resume.Resume;

        _dbContext.Transcripts.Update(transcript);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _backgroundTaskQueue.QueueBackgroundWorkItem(async (sp, token) =>
        {
            var mediaJobService = sp.GetRequiredService<IMediaJobService>();

            await mediaJobService.CreateJob(new KeywordExtractionMediaProcessingJob()
            {
                ParentJobId = notification.Job.Id,
                SourceText = transcript.TranscriptText,
                Context = transcript.Summary
            }, cancellationToken);
        });
    }
}
