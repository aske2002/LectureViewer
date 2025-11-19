using System.Text.Json.Serialization;

namespace backend.Infrastructure.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WhisperEventType
{
    Transcript,
    Progress,
    Log,
    End
}