using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;

namespace backend.Application.Lectures.EventsHandlers;

public class TranscriptionCompletedEventHandler : INotificationHandler<JobSuccessEvent<TranscriptionMediaProcessingJob>>
{
    private readonly ILogger<TranscriptionCompletedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public TranscriptionCompletedEventHandler(
        ILogger<TranscriptionCompletedEventHandler> logger,
        IApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(JobSuccessEvent<TranscriptionMediaProcessingJob> notification, CancellationToken cancellationToken)
    {
        var transcript = notification.Job.Transcript;
        var lecturejob = await notification.Job.GetJobOfType<LectureProcessingJob>(_dbContext, cancellationToken);

        if (transcript == null)
        {
            _logger.LogWarning("TranscriptionCompletedEventHandler: Transcript is null for job {JobId}", notification.Job.Id);
            return;
        }

        if (lecturejob == null)
        {
            return;
        }

        await _dbContext.LectureProcessingJobs.Entry(lecturejob).Reference(l => l.LectureContent).LoadAsync(cancellationToken);
        
        lecturejob.LectureContent.TranscriptId = transcript.Id;
        lecturejob.LectureContent.Transcript = transcript;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
