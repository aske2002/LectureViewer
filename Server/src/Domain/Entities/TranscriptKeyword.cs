using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class TranscriptKeyword : BaseAuditableEntity<TranscriptKeywordId>
{
    public required TranscriptId TranscriptId { get; set; }
    public required Transcript Transcript { get; set; }
    public IList<TranscriptKeywordOccurrence> Occurrences { get; set; } = new List<TranscriptKeywordOccurrence>();
    public required string Keyword { get; set; }
}
