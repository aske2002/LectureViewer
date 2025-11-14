using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class LectureTranscriptTimestamp : BaseAuditableEntity<LectureTranscriptTimestampId>
{
    public required LectureTranscriptId LectureTranscriptId { get; set; }
    public required LectureTranscript LectureTranscript { get; set; }
    public required TimeSpan Timestamp { get; set; }
    public required string Text { get; set; }
}
