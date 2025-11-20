using System.Text.Json.Serialization;

namespace backend.Infrastructure.Enums;

public static class WhisperEventType
{
    public const string Transcript = "transcript";
    public const string Progress = "progress";
    public const string Log = "log";
    public const string End = "end";
    public const string Connected = "connected";
}