// using backend.Domain.Events;
// using Microsoft.Extensions.Logging;
// using backend.Application.Common.Interfaces;
// using backend.Domain.Entities;

// namespace backend.Application.Keywords.EventsHandlers;

// public class KeywordsExtractedEventHandler : INotificationHandler<JobSuccessEvent<KeywordExtractionMediaProcessingJob>>
// {
//     private readonly ILogger<KeywordsExtractedEventHandler> _logger;
//     private readonly IApplicationDbContext _dbContext;

//     public KeywordsExtractedEventHandler(
//         ILogger<KeywordsExtractedEventHandler> logger,
//         IApplicationDbContext dbContext)
//     {
//         _logger = logger;
//         _dbContext = dbContext;
//     }

//     public async Task Handle(JobSuccessEvent<KeywordExtractionMediaProcessingJob> notification, CancellationToken cancellationToken)
//     {
//         var keywords = notification.Job.ExtractedKeywords;
//         await _dbContext.Entry(notification.Job).Reference(j => j.ExtractedKeywords).LoadAsync(cancellationToken);
//         var lecturejob = await notification.Job.GetJobOfType<LectureProcessingJob>(_dbContext, cancellationToken);

//         if (keywords == null || !keywords.Any())
//         {
//             _logger.LogWarning("TranscriptionCompletedEventHandler: Transcript is null for job {JobId}", notification.Job.Id);
//             return;
//         }

//         if (lecturejob == null)
//         {
//             return;
//         }

        
//     }
// }
