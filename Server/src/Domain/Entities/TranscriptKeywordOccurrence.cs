using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class TranscriptKeywordOccurrence : BaseAuditableEntity<TranscriptKeywordOccurrenceId>
{
    public required KeywordId KeywordId { get; set; }
    public required Keyword Keyword { get; set; }
    public required TranscriptId TranscriptId { get; set; }
    public required Transcript Transcript { get; set; }
    public required TimeSpan From { get; set; }
    public required TimeSpan To { get; set; }
    public required string SurroundingText { get; set; }
}
