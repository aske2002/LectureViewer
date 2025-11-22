using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class LectureContent : BaseAuditableEntity<LectureContentId>
{
    public required LectureId LectureId { get; init; }
    public required Lecture Lecture { get; init; } = null!;
    public LectureProcessingJob? Job { get; init; }
    public TranscriptId? TranscriptId { get; init; }
    public Transcript? Transcript { get; init; }
    public required bool IsMainContent { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required ResourceId ResourceId { get; init; }
    public required Resource Resource { get; init; }
    public required LectureContentType ContentType { get; init; }
}