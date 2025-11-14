using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class LectureTranscriptKeywordOccurrence : BaseAuditableEntity<LectureTranscriptKeywordOccurrenceId>
{
    public required LectureTranscriptKeywordId LectureTranscriptKeywordId { get; set; }
    public required LectureTranscriptKeyword LectureTranscriptKeyword { get; set; }
    public required TimeSpan OccurrenceTime { get; set; }
    public required string SurroundingText { get; set; }
}
