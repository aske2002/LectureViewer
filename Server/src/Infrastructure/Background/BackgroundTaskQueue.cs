using System.Threading.Channels;
using backend.Application.Common.Interfaces;

namespace backend.Infrastructure.Background;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<IServiceProvider, CancellationToken, Task>> _queue;

    public BackgroundTaskQueue(int capacity = 100)
    {
        _queue = Channel.CreateBounded<Func<IServiceProvider, CancellationToken, Task>>(
            new BoundedChannelOptions(capacity) { FullMode = BoundedChannelFullMode.Wait });
    }

    public void QueueBackgroundWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem)
        => _queue.Writer.TryWrite(workItem);

    public async Task<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken token)
        => await _queue.Reader.ReadAsync(token);
}

