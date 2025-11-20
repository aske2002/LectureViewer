using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public interface IMediaProcessingJob
{
    MediaJobType JobType { get; set; }
    JobStatus Status { get; }
    DateTimeOffset? StartedAt { get; set; }
    DateTimeOffset? CompletedAt { get; set; }
    IList<MediaProcessingJobAttempt> Attempts { get; }
    IList<MediaProcessingJob> DependentJobs { get; }
    MediaProcessingJob? ParentJob { get; set; }
    MediaProcessingJobId? ParentJobId { get; set; }
    int MaxRetries { get; set; }
    string? ErrorMessage { get; set; }
}

public interface IHasOutputResource
{
    ResourceId? OutputResourceId { get; set; }
    Resource? OutputResource { get; set; }
}

public interface IHasInputResource
{
    ResourceId? InputResourceId { get; set; }
    Resource? InputResource { get; set; }
}

public abstract class MediaProcessingJob : BaseAuditableEntity<MediaProcessingJobId>, IMediaProcessingJob
{
    public IList<MediaProcessingJob> DependentJobs { get; private set; } = new List<MediaProcessingJob>();
    public IList<MediaProcessingJobAttempt> Attempts { get; } = new List<MediaProcessingJobAttempt>();
    public MediaProcessingJobId? ParentJobId { get; set; }
    public MediaProcessingJob? ParentJob { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public MediaJobType JobType { get; set; }
    public int MaxRetries { get; set; } = 3;
    public string? ErrorMessage { get; set; }
    public bool Failed{ get; set; } = false;
    public JobStatus Status => Attempts.Where(a => a.Status == JobStatus.InProgress).Any()
        ? JobStatus.InProgress
        : Attempts.Where(a => a.Status == JobStatus.Failed).Count() >= MaxRetries
            ? JobStatus.Failed
            : Attempts.Where(a => a.Status == JobStatus.Completed).Any()
                ? JobStatus.Completed
                : JobStatus.Pending;
}

public abstract class LectureRelatedMediaProcessingJob : MediaProcessingJob
{
    public required LectureContentId LectureContentId { get; set; }
    public required LectureContent LectureContent { get; set; }
}

public class KeywordExtractionMediaProcessingJob : LectureRelatedMediaProcessingJob
{

}

public class CategoryClassificationMediaProcessingJob : LectureRelatedMediaProcessingJob
{

}

public class TranscriptionMediaProcessingJob : LectureRelatedMediaProcessingJob
{

}

public class FlashcardGenerationMediaProcessingJob : LectureRelatedMediaProcessingJob
{

}

public class ThumbnailExtractionMediaProcessingJob : MediaProcessingJob,IHasOutputResource, IHasInputResource
{
    public int? Width { get; set; }
    public int? Height { get; set; }
    public ResourceId? InputResourceId { get; set; }
    public Resource? InputResource { get; set; }
    public ResourceId? OutputResourceId { get; set; }
    public Resource? OutputResource { get; set; }
}

public class MediaTranscodingMediaProcessingJob : MediaProcessingJob, IHasOutputResource, IHasInputResource
{
    public ResourceId? InputResourceId { get; set; }
    public Resource? InputResource { get; set; }
    public ResourceId? OutputResourceId { get; set; }
    public Resource? OutputResource { get; set; }
    public required string TargetFormat { get; set; }
    public int? TargetWidth { get; set; }
    public int? TargetHeight { get; set; }
    public int? TargetBitrateKbps { get; set; }
}

public class OfficeConversionMediaProcessingJob : MediaProcessingJob, IHasOutputResource, IHasInputResource
{
    public ResourceId? InputResourceId { get; set; }
    public Resource? InputResource { get; set; }
    public ResourceId? OutputResourceId { get; set; }
    public Resource? OutputResource { get; set; }
    public required string TargetFormat { get; set; }
}