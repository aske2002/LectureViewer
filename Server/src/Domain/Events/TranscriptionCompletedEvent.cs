using backend.Domain.Identifiers;
using MediatR;

namespace backend.Domain.Events;

public record TranscriptionCompletedEvent(Transcript Transcript) : INotification;