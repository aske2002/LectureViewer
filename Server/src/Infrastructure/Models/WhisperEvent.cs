
using System.Text.Json.Serialization;
using backend.Infrastructure.Enums;

namespace backend.Infrastructure.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "status")]
[JsonDerivedType(typeof(WhisperTranscriptEvent), WhisperEventType.Transcript)]
[JsonDerivedType(typeof(WhisperProgressEvent), WhisperEventType.Progress)]
[JsonDerivedType(typeof(WhisperLogEvent), WhisperEventType.Log)]
[JsonDerivedType(typeof(WhisperEndEvent), WhisperEventType.End)]
[JsonDerivedType(typeof(WhisperConnectEvent), WhisperEventType.Connected)]
abstract class WhisperEvent
{
}

abstract class WhisperEventBase : WhisperEvent
{
    public required long Timestamp { get; set; }

}

class WhisperConnectEvent : WhisperEvent
{
}

class WhisperTranscriptEvent : WhisperEventBase
{
    public required double From { get; set; }
    public required double To { get; set; }
    public required string Text { get; set; }
}

class WhisperProgressEvent : WhisperEventBase
{
    public required double Progress { get; set; }
}

class WhisperLogEvent : WhisperEventBase
{
    public required string Message { get; set; }
}

class WhisperEndEvent : WhisperEventBase
{
}
