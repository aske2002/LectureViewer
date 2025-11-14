using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using backend.Domain.Entities;

namespace backend.Application.Lectures.EventsHandlers;

public class MediaUploadedHandler : INotificationHandler<MediaUploadedEvent>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ILogger<MediaUploadedHandler> _logger;

    public MediaUploadedHandler(
        IBackgroundTaskQueue backgroundTaskQueue,
        ILogger<MediaUploadedHandler> logger)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
    }

    public Task Handle(MediaUploadedEvent notification, CancellationToken cancellationToken)
    {
        _backgroundTaskQueue.QueueBackgroundWorkItem(async (sp, token) =>
        {
            var documentMimeTypes = new[] {
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.ms-powerpoint",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            var videoMimeTypes = new[] {
                "video/mp4",
                "video/avi",
                "video/mov",
                "video/mpeg",
                "video/quicktime"
            };

            var audioMimeTypes = new[] {
                "audio/mpeg",
                "audio/wav",
                "audio/ogg",
                "audio/flac"
            };

            var jobService = sp.GetRequiredService<IMediaJobService>();

            // fetch lectureContent fresh if needed
            var db = sp.GetRequiredService<ICourseService>();
            var lectureContent = await db.GetLectureContentDetailsAsync(
                notification.CourseId,
                notification.LectureId,
                notification.LectureContentId);

            if (lectureContent == null)
            {
                _logger.LogWarning("Lecture content {LectureContentId} not found for Lecture {LectureId} in Course {CourseId}",
                    notification.LectureContentId, notification.LectureId, notification.CourseId);
                return;
            }
            else if (documentMimeTypes.Contains(lectureContent.Resource.MimeType))
            {
                // Create document processing job
                var job = new OfficeConversionMediaProcessingJob
                {
                    InputResourceId = lectureContent.ResourceId,
                    InputResource = lectureContent.Resource,
                    TargetFormat = "pdf",
                    JobType = MediaJobType.OfficeConversion,
                };

                await jobService.CreateJob(job, token);
            }
            else if (videoMimeTypes.Contains(lectureContent.Resource.MimeType) ||
                     audioMimeTypes.Contains(lectureContent.Resource.MimeType))
            {
                var conversionJob = new MediaConversionMediaProcessingJob
                {
                    InputResourceId = lectureContent.ResourceId,
                    InputResource = lectureContent.Resource,
                    TargetFormat = "mp3",
                    JobType = MediaJobType.MediaConversion,
                };

                var thumbnailJob = new ThumbnailExtractionMediaProcessingJob
                {
                    LectureContentId = lectureContent.Id,
                    LectureContent = lectureContent,
                    JobType = MediaJobType.ThumbnailExtraction,
                };

                var transcriptionJob = new TranscriptionMediaProcessingJob
                {
                    LectureContentId = lectureContent.Id,
                    ParentJob = conversionJob,
                    LectureContent = lectureContent,
                    JobType = MediaJobType.Transcription,
                };

                var categoryJob = new CategoryClassificationMediaProcessingJob
                {
                    LectureContentId = lectureContent.Id,
                    ParentJob = transcriptionJob,
                    LectureContent = lectureContent,
                    JobType = MediaJobType.CategoryClassification,
                };

                var keywordJob = new KeywordExtractionMediaProcessingJob
                {
                    LectureContentId = lectureContent.Id,
                    ParentJob = transcriptionJob,
                    LectureContent = lectureContent,
                    JobType = MediaJobType.KeywordExtraction,
                };

                var flashcardJob = new FlashcardGenerationMediaProcessingJob
                {
                    LectureContentId = lectureContent.Id,
                    ParentJob = keywordJob,
                    LectureContent = lectureContent,
                    JobType = MediaJobType.FlashcardGeneration,
                };

                await jobService.CreateJob(conversionJob, token);
                await jobService.CreateJob(thumbnailJob, token);
                await jobService.CreateJob(transcriptionJob, token);
                await jobService.CreateJob(categoryJob, token);
                await jobService.CreateJob(keywordJob, token);
                await jobService.CreateJob(flashcardJob, token);
            }
        });

        _logger.LogInformation("Queued job creation for Lecture {LectureId}", notification.LectureId);
        return Task.CompletedTask;
    }
}
