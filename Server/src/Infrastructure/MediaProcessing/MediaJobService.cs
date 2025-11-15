
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Services;

public class MediaJobService : IMediaJobService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MediaJobService> _logger;

    public MediaJobService(ApplicationDbContext db, ILogger<MediaJobService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task CreateJob(MediaProcessingJob job, CancellationToken token)
    {

        await _db.MediaProcessingJobs.AddAsync(job, token);
        await _db.SaveChangesAsync(token);
    }

    public async Task<(MediaProcessingJob job, MediaProcessingJobAttempt attempt)?> TryAcquireNextPendingJobAsync(CancellationToken token = default)
    {
        var job = await _db.MediaProcessingJobs
            .Where(j =>
                j.Attempts.Count(a => a.Status == JobStatus.Failed) < j.MaxRetries &&
                !j.Attempts.Any(a => a.Status == JobStatus.Completed) &&
                (j.ParentJob == null || j.ParentJob.Attempts.Any(pa => pa.Status == JobStatus.Completed)) &&
                !j.Failed
            )
            .Include(j => j.Attempts)
                .ThenInclude(a => a.Logs)
            .OrderBy(j => j.Created)
            .FirstOrDefaultAsync(token);

        if (job is null)
            return null;

        var attempt = job.Attempts.FirstOrDefault(a => a.Status == JobStatus.InProgress);
        if (attempt is null)
        {
            attempt = new MediaProcessingJobAttempt
            {
                JobId = job.Id,
                Job = job,
                StartedAt = DateTimeOffset.UtcNow,
                Status = JobStatus.InProgress
            };
            await _db.MediaProcessingJobAttempts.AddAsync(attempt, token);
        }
        else
        {
            attempt.StartedAt = DateTimeOffset.UtcNow;
        }

        job.StartedAt = job.StartedAt ?? DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(token);
        _logger.LogInformation("Acquired job {JobId} for processing.", job.Id);
        return (job, attempt);
    }
    public async Task MarkAttemptCompleted(MediaProcessingJobAttemptId jobId, CancellationToken token = default)
    {
        var attempt = await _db.MediaProcessingJobAttempts
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == jobId, cancellationToken: token);

        if (attempt is null)
            return;

        attempt.Status = JobStatus.Completed;
        attempt.CompletedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(token);
        _logger.LogInformation("Job {JobId} marked as completed.", attempt.JobId);
    }

    public async Task MarkJobFailedAsync(MediaProcessingJobId jobId, string errorMessage, CancellationToken token = default)
    {
        var job = await _db.MediaProcessingJobs
            .Include(j => j.Attempts)
            .Include(j => j.DependentJobs)
            .FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken: token);

        if (job is null)
            return;

        foreach (var dependenJob in job.DependentJobs)
        {
            await MarkJobFailedAsync(dependenJob.Id, "Parent job failed.", token);
        }

        await _db.SaveChangesAsync(token);
        _logger.LogWarning("Job {JobId} failed: {Error}", jobId, errorMessage);
    }

    public async Task MarkAttemptFailedAsync(MediaProcessingJobAttemptId jobId, string errorMessage, CancellationToken token = default)
    {
        var attempt = await _db.MediaProcessingJobAttempts
            .Include(a => a.Job)
                .ThenInclude(a => a.DependentJobs)
            .FirstOrDefaultAsync(a => a.Id == jobId, cancellationToken: token);

        if (attempt is null)
            return;

        attempt.Status = JobStatus.Failed;
        attempt.ErrorMessage = errorMessage;
        attempt.CompletedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(token);

        if (attempt.Job.Attempts.Count(a => a.Status == JobStatus.Failed) >= attempt.Job.MaxRetries)
        {
            _logger.LogWarning("Job {JobId} has reached maximum retry attempts and is marked as failed.", attempt.JobId);
            await MarkJobFailedAsync(attempt.JobId, "Maximum retry attempts reached.", token);
        }

        _logger.LogWarning("Job {JobId} failed: {Error}", attempt.JobId, errorMessage);
    }
}
