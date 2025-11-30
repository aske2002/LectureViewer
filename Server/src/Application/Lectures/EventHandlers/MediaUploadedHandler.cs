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
            var jobService = sp.GetRequiredService<IMediaJobService>();
            var courseService = sp.GetRequiredService<ICourseService>();
            var lectureContent = await courseService.GetLectureContentDetailsAsync(notification.CourseId, notification.LectureContentId);

            if (lectureContent == null)
            {
                _logger.LogWarning("Lecture content not found for LectureContentId: {LectureContentId}", notification.LectureContentId);
                return;
            }

            var job = new LectureProcessingJob
            {
                LectureContentId = lectureContent.Id,
                LectureContent = lectureContent,
                JobType = MediaJobType.LectureProcessing,
            };

            await jobService.CreateJob(job, token);
        });

        _logger.LogInformation("Queued job creation for LectureContent {LectureContentId}", notification.LectureContentId);
        return Task.CompletedTask;
    }
}
