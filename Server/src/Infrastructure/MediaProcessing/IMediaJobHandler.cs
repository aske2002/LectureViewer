using backend.Domain.Entities;
using backend.Infrastructure.Data;

namespace backend.Infrastructure.MediaProcessing;

public interface IMediaJobHandler
{
    Task HandleAsync(IMediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token);
}

public interface IMediaJobHandler<TJob> : IMediaJobHandler
    where TJob : IMediaProcessingJob
{
    Task HandleAsync(TJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token);
}
public abstract class MediaJobHandlerBase<TJob> : IMediaJobHandler<TJob>
    where TJob : IMediaProcessingJob
{
    public abstract Task HandleAsync(TJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token);

    async Task IMediaJobHandler.HandleAsync(IMediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token)
    {
        if (job is not TJob typedJob)
        {
            throw new ArgumentException($"Invalid job type. Expected {typeof(TJob).Name}, got {job.GetType().Name}");
        }

        await HandleAsync(typedJob, attempt, inputResource, token);
    }
}