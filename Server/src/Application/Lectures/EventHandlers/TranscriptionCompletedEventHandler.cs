using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;

namespace backend.Application.Lectures.EventsHandlers;

public class TranscriptionCompletedEventHandler : INotificationHandler<TranscriptionCompletedEvent>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ILogger<TranscriptionCompletedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public TranscriptionCompletedEventHandler(
        IBackgroundTaskQueue backgroundTaskQueue,
        ILogger<TranscriptionCompletedEventHandler> logger,
        IApplicationDbContext dbContext)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(TranscriptionCompletedEvent notification, CancellationToken cancellationToken)
    {
        var transcript = notification.Transcript;
        var lecturejob = await transcript.Job.GetJobOfType<LectureProcessingJob>(_dbContext, cancellationToken);
        
        if (lecturejob == null)
        {
            return;
        }

        await _dbContext.LectureProcessingJobs.Entry(lecturejob).Reference(l => l.LectureContent).LoadAsync(cancellationToken);
        await _dbContext.LectureContents.Entry(lecturejob.LectureContent).Reference(lc => lc.Lecture).LoadAsync(cancellationToken);
        lecturejob.LectureContent.Lecture.Transcripts.Add(transcript);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
