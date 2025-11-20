
using System.Text.Json.Serialization;
using backend.Infrastructure.Enums;

namespace backend.Infrastructure.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(WhisperTranscriptEvent), nameof(WhisperEventType.Transcript))]
[JsonDerivedType(typeof(WhisperProgressEvent), nameof(WhisperEventType.Progress))]
[JsonDerivedType(typeof(WhisperLogEvent), nameof(WhisperEventType.Log))]
[JsonDerivedType(typeof(WhisperEndEvent), nameof(WhisperEventType.End))]
abstract class WhisperEvent
{
    public abstract WhisperEventType Type { get; }
    public required int Timestamp { get; set; } 
}

class WhisperTranscriptEvent : WhisperEvent
{
    public override WhisperEventType Type => WhisperEventType.Transcript;
    public required double From { get; set; }
    public required double To { get; set; }
    public required string Text { get; set; }
}

class WhisperProgressEvent : WhisperEvent
{
    public override WhisperEventType Type => WhisperEventType.Progress;
    public required double Progress { get; set; }
}

class WhisperLogEvent : WhisperEvent
{
    public override WhisperEventType Type => WhisperEventType.Log;
    public required string Message { get; set; }
}

class WhisperEndEvent : WhisperEvent
{
    public override WhisperEventType Type => WhisperEventType.End;
}
