using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class TranscriptItem : BaseAuditableEntity<TranscriptItemId>
{
    public TranscriptId TranscriptId { get; set; } = TranscriptId.Default();
    public Transcript Transcript { get; set; } = null!;
    public required TimeSpan From { get; set; }
    public required TimeSpan To { get; set; }
    public required double Confidence { get; set; }
    public required string Text { get; set; }
}
