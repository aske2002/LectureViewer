using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class MediaProcessingJobLog : BaseAuditableEntity<MediaProcessingJobLogId>
{
    public MediaProcessingJobAttemptId AttemptId { get; init; } = MediaProcessingJobAttemptId.Default();
    public MediaProcessingJobAttempt Attempt { get; init; } = null!;
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset LoggedAt { get; set; }
    public JobLogLevel Level { get; set; } = JobLogLevel.Info;
    public int? ProgressPercentage { get; set; }
}