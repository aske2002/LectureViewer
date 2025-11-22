using backend.Domain.Entities;
using backend.Infrastructure.Data;

namespace backend.Infrastructure.MediaProcessing;

public interface IMediaJobHandler
{
    Task HandleAsync(IMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token);
}

public interface IMediaJobHandler<TJob> : IMediaJobHandler
    where TJob : IMediaProcessingJob
{
    Task HandleAsync(TJob job, MediaProcessingJobAttempt attempt, CancellationToken token);
}
public abstract class MediaJobHandlerBase<TJob> : IMediaJobHandler<TJob>
    where TJob : IMediaProcessingJob
{
    protected readonly ApplicationDbContext DbContext;
    protected MediaJobHandlerBase(ApplicationDbContext db)
    {
        DbContext = db;
    }

    public abstract Task HandleAsync(TJob job, MediaProcessingJobAttempt attempt, CancellationToken token);

    protected async Task<Resource?> FirstResourceOrDefaultAsync(MediaProcessingJob job, Func<Resource, bool> predicate, CancellationToken token)
    {
        if (job is IHasInputResource jobWithInput && jobWithInput.InputResource != null)
        {
            if (predicate(jobWithInput.InputResource))
            {
                return jobWithInput.InputResource;
            }
        }

        var allPreviousJobs = await ListAllPreviousJobsAsync(job, token);

        return allPreviousJobs.OfType<IHasOutputResource>()
            .Select(pj => pj.OutputResource)
            .Where(r => r != null)
            .Cast<Resource>()
            .OrderByDescending(r => r.Created)
            .ToList().FirstOrDefault(predicate);
    }

    protected async Task<ICollection<MediaProcessingJob>> ListAllPreviousJobsAsync(MediaProcessingJob job, CancellationToken token)
    {
        var jobs = new List<MediaProcessingJob>();

        await DbContext.MediaProcessingJobs.Entry(job).Reference(j => j.ParentJob).LoadAsync(token);

        if (job.ParentJob is not null)
        {
            if (job.ParentJob is IHasInputResource parentJobWithInput)
            {
                await DbContext.Entry(parentJobWithInput).Reference(r => r.InputResource).LoadAsync(token);
            }

            if (job.ParentJob is IHasOutputResource parentJobWithOutput)
            {
                await DbContext.Entry(parentJobWithOutput).Reference(r => r.OutputResource).LoadAsync(token);
            }

            jobs.Add(job.ParentJob);
            var parentJobs = await ListAllPreviousJobsAsync(job.ParentJob, token);
            jobs.AddRange(parentJobs);
        }

        return jobs;
    }
    
    async Task IMediaJobHandler.HandleAsync(IMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        if (job is not TJob typedJob)
        {
            throw new ArgumentException($"Invalid job type. Expected {typeof(TJob).Name}, got {job.GetType().Name}");
        }

        await HandleAsync(typedJob, attempt, token);
    }
}