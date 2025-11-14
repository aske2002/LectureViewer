using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class LectureTranscriptKeyword : BaseAuditableEntity<LectureTranscriptKeywordId>
{
    public required LectureTranscriptId LectureTranscriptId { get; set; }
    public required LectureTranscript LectureTranscript { get; set; }
    public IList<LectureTranscriptKeywordOccurrence> Occurrences { get; set; } = new List<LectureTranscriptKeywordOccurrence>();
    public required string Keyword { get; set; }
}
