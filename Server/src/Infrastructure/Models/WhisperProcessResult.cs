using System.Text.Json.Serialization;
using backend.Infrastructure.Enums;

namespace backend.Infrastructure.MediaProcessing.Transcription.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Status))]
[JsonDerivedType(typeof(WhisperProcessSuccessResult), nameof(WhisperProcessResultType.Success))]
[JsonDerivedType(typeof(WhisperProcessErrorResult), nameof(WhisperProcessResultType.Error))]
[JsonDerivedType(typeof(WhisperProcessRunningResult), nameof(WhisperProcessResultType.Running))]
abstract class WhisperProcessResult
{
    public abstract WhisperProcessResultType Status { get; }
    public List<string> Output { get; set; } = [];
}

class WhisperProcessSuccessResult : WhisperProcessResult
{
    public override WhisperProcessResultType Status => WhisperProcessResultType.Success;
    public required WhisperOutput Data { get; set; }
}

class WhisperProcessErrorResult : WhisperProcessResult
{
    public override WhisperProcessResultType Status => WhisperProcessResultType.Error;
    public required string ErrorMessage { get; set; }
}

class WhisperProcessRunningResult : WhisperProcessResult
{
    public override WhisperProcessResultType Status => WhisperProcessResultType.Running;
    public required int ProgressPercentage { get; set; }
}
