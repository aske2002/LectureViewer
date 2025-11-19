using System.Text.Json.Serialization;

namespace backend.Infrastructure.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WhisperProcessResultType
{
    Success,
    Error,
    Running
}