using System.Text.Json.Serialization;
using backend.Infrastructure.Enums;

namespace backend.Infrastructure.MediaProcessing.Transcription.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "status")]
[JsonDerivedType(typeof(WhisperProcessSuccessResult), WhisperProcessResultType.Success)]
[JsonDerivedType(typeof(WhisperProcessErrorResult), WhisperProcessResultType.Error)]
[JsonDerivedType(typeof(WhisperProcessRunningResult), WhisperProcessResultType.Running)]
abstract class WhisperProcessResult
{
    public List<string> Output { get; set; } = [];
}

class WhisperProcessSuccessResult : WhisperProcessResult
{
    public required WhisperOutput Data { get; set; }
}

class WhisperProcessErrorResult : WhisperProcessResult
{
    public required string ErrorMessage { get; set; }
}

class WhisperProcessRunningResult : WhisperProcessResult
{
    public required int ProgressPercentage { get; set; }
}
