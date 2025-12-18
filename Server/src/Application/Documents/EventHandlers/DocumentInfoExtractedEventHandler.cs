using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;

namespace backend.Application.Documents.EventsHandlers;

public class DocumentInfoExtractedEventHandler : INotificationHandler<JobSuccessEvent<DocumentInfoExtractionJob>>
{
    private readonly ILogger<DocumentInfoExtractedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public DocumentInfoExtractedEventHandler(
        ILogger<DocumentInfoExtractedEventHandler> logger,
        IApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(JobSuccessEvent<DocumentInfoExtractionJob> notification, CancellationToken cancellationToken)
    {
        var infoJob = notification.Job;
        await _dbContext.Entry(notification.Job).Reference(j => j.Document).LoadAsync(cancellationToken);
        var document = infoJob.Document;

        if (document == null)
        {
            _logger.LogWarning("DocumentInfoExtractedEventHandler: Document is null for job {JobId}", notification.Job.Id);
            return;
        }

        await _dbContext.Entry(document).Reference(d => d.LectureContent).LoadAsync(cancellationToken);
        var lectureContent = document.LectureContent;

        if (lectureContent == null)
        {
            _logger.LogWarning("DocumentInfoExtractedEventHandler: LectureContent is null for document {DocumentId}", document.Id);
            return;
        }

        lectureContent.Summary = infoJob.Summary;

        _dbContext.LectureContents.Update(lectureContent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
