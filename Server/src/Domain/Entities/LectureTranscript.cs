using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class LectureTranscript : BaseAuditableEntity<LectureTranscriptId>
{
    public required LectureId LectureId { get; set; }
    public required Lecture Lecture { get; set; }
    public required LectureContentId SourceId { get; set; }
    public required LectureContent Source { get; set; }
    public MediaProcessingJobId? ArtifactFromJobId { get; set; }
    public MediaProcessingJob? ArtifactFromJob { get; set; }
    public IList<LectureTranscriptTimestamp> Timestamps { get; set; } = new List<LectureTranscriptTimestamp>();
    public IList<LectureTranscriptKeyword> Keywords { get; set; } = new List<LectureTranscriptKeyword>();
    public required string Summary { get; set; }
    public required string TranscriptText { get; set; }
}
