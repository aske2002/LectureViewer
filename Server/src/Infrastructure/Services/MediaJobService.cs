
using backend.Domain.Entities;
using backend.Domain.Events;
using backend.Domain.Identifiers;
using backend.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Services;

public class MediaJobService : IMediaJobService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MediaJobService> _logger;
    private readonly IMediator _mediator;

    public MediaJobService(ApplicationDbContext db, ILogger<MediaJobService> logger, IMediator mediator)
    {
        _db = db;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task CreateJob(MediaProcessingJob job, CancellationToken token)
    {

        await _db.MediaProcessingJobs.AddAsync(job, token);
        await _db.SaveChangesAsync(token);
    }

    public async Task<(MediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource)?> TryAcquireNextPendingJobAsync(CancellationToken token = default)
    {
        var job = await _db.MediaProcessingJobs
            .Where(j =>
                j.Attempts.Count(a => a.Status == JobStatus.Failed) < j.MaxRetries &&
                !j.Attempts.Any(a => a.Status == JobStatus.Completed) &&
                (j.ParentJob == null || j.ParentJob.Attempts.Any(pa => pa.Status == JobStatus.Completed)) &&
                !j.Failed
            )
            .Include(j => j.ParentJob)
            .Include(j => j.Attempts)
                .ThenInclude(a => a.Logs)
            .OrderBy(j => j.Created)
            .FirstOrDefaultAsync(token);

        Resource? resource = null;

        if (job is null)
            return null;

        if (job is IHasInputResource parentJobWithInput)
        {
            await _db.Entry(parentJobWithInput).Reference(r => r.InputResource).LoadAsync(token);
            if (parentJobWithInput.InputResource is not null)
            {
                resource = parentJobWithInput.InputResource;
            }
            else if (parentJobWithInput.InputMimeType is not null)
            {
                resource = await job.GetMatchingResource(_db, r =>
                {
                    var mimeType = MimeTypeHelpers.FromMimeType(r.MimeType);
                    if (mimeType == null)
                    {
                        return false;
                    }
                    return parentJobWithInput.InputMimeType.Value.HasFlag(mimeType);
                }, token);
            }
        }

        if (job is IHasOutputResource parentJobWithOutput)
        {
            await _db.Entry(parentJobWithOutput).Reference(r => r.OutputResource).LoadAsync(token);
        }

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
        return (job, attempt, resource);
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
        await _mediator.Publish(JobEventFactory.CreateSuccessEvent(attempt.Job), token);
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
        await _mediator.Publish(JobEventFactory.CreateErrorEvent(job), token);
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
