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

    public ResumeExtractedEventHandler(
        ILogger<ResumeExtractedEventHandler> logger,
        IApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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
    }
}
