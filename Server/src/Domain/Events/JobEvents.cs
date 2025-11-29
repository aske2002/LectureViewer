using MediatR;

namespace backend.Domain.Events;

public static class JobEventFactory
{
    public static INotification CreateSuccessEvent(MediaProcessingJob job)
    {
        var genericType = typeof(JobSuccessEvent<>).MakeGenericType(job.GetType());
        var instance = Activator.CreateInstance(genericType, [job]) as INotification;
        if (instance is null)
        {
            throw new InvalidOperationException("Could not create JobSuccessEvent instance.");
        }
        return instance;
    }

    public static INotification CreateErrorEvent(MediaProcessingJob job)
    {
        var genericType = typeof(JobErrorEvent<>).MakeGenericType(job.GetType());
        var instance = Activator.CreateInstance(genericType, [job]) as INotification;
        if (instance is null)
        {
            throw new InvalidOperationException("Could not create JobErrorEvent instance.");
        }
        return instance;
    }
}

public record JobSuccessEvent<T>(T Job) : INotification where T : MediaProcessingJob;
public record JobErrorEvent<T>(T Job) : INotification where T : MediaProcessingJob;