using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class TranscriptKeywordOccurrence : BaseAuditableEntity<TranscriptKeywordOccurrenceId>
{
    public required TranscriptKeywordId TranscriptKeywordId { get; set; }
    public required TranscriptKeyword TranscriptKeyword { get; set; }
    public required TimeSpan OccurrenceTime { get; set; }
    public required string SurroundingText { get; set; }
}
