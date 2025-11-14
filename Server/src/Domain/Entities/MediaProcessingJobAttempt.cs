using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class MediaProcessingJobAttempt : BaseAuditableEntity<MediaProcessingJobAttemptId>
{
    public IList<MediaProcessingJobLog> Logs { get; } = new List<MediaProcessingJobLog>();
    public MediaProcessingJobId JobId { get; set; } = MediaProcessingJobId.Default();
    public MediaProcessingJob Job { get; set; } = null!;
    public JobStatus Status { get; set; } = JobStatus.Pending;
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
