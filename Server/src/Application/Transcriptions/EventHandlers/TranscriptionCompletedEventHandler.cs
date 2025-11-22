using backend.Domain.Events;
using Microsoft.Extensions.Logging;
using backend.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using backend.Domain.Entities;

namespace backend.Application.Transcriptions.EventsHandlers;

public class TranscriptionCompletedEventHandler : INotificationHandler<TranscriptionCompletedEvent>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ILogger<TranscriptionCompletedEventHandler> _logger;

    public TranscriptionCompletedEventHandler(
        IBackgroundTaskQueue backgroundTaskQueue,
        ILogger<TranscriptionCompletedEventHandler> logger)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
    }

    public Task Handle(TranscriptionCompletedEvent notification, CancellationToken cancellationToken)
    {
        _backgroundTaskQueue.QueueBackgroundWorkItem((sp, token) =>
        {
            return Task.CompletedTask;
        });

        return Task.CompletedTask;
    }
}
