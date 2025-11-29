using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;

namespace backend.Application.Transcriptions.EventsHandlers;

public class ResumeExtractedEventHandler : INotificationHandler<JobSuccessEvent<ResumeExtractionMediaProcessingJob>>
{
    private readonly IMediaJobService _mediaJobService;
    private readonly ILogger<ResumeExtractedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public ResumeExtractedEventHandler(
        IMediaJobService mediaJobService,
        ILogger<ResumeExtractedEventHandler> logger,
        IApplicationDbContext dbContext)
    {
        _mediaJobService = mediaJobService;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(JobSuccessEvent<ResumeExtractionMediaProcessingJob> notification, CancellationToken cancellationToken)
    {
        var resume = notification.Job;
        var transcriptionJob = await notification.Job.GetJobOfType<TranscriptionMediaProcessingJob>(_dbContext, cancellationToken);

        if (transcriptionJob == null || transcriptionJob.Transcript == null)
        {
            return;
        }

        transcriptionJob.Transcript.Summary = resume.Resume;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
