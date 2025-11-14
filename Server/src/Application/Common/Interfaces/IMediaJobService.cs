using backend.Domain.Entities;
using backend.Domain.Identifiers;

public interface IMediaJobService
{
    Task CreateJob(MediaProcessingJob job, CancellationToken token);

    /// <summary>
    /// Attempts to start a pending job, marking it as Processing.
    /// </summary>
    Task<(MediaProcessingJob job, MediaProcessingJobAttempt attempt)?> TryAcquireNextPendingJobAsync(CancellationToken token = default);

    /// <summary>
    /// Marks a job as completed.
    /// </summary>
    Task MarkAttemptCompleted(MediaProcessingJobAttemptId jobId, CancellationToken token = default);

    /// <summary>
    /// Marks a job as failed, incrementing retry count.
    /// </summary>
    Task MarkAttemptFailedAsync(MediaProcessingJobAttemptId jobId, string errorMessage, CancellationToken token = default);
}