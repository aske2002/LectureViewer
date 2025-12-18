using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Transcript : BaseAuditableEntity<TranscriptId>
{
    public required ResourceId SourceId { get; set; }
    public required Resource Source { get; set; }
    public required MediaProcessingJobId JobId { get; set; }
    public required TranscriptionMediaProcessingJob Job { get; set; }
    public IList<TranscriptItem> Items { get; set; } = new List<TranscriptItem>();
    public IList<TranscriptKeywordOccurrence> Keywords { get; set; } = new List<TranscriptKeywordOccurrence>();
    public string? Summary { get; set; }
    public required string Language { get; set; }
    public required string TranscriptText { get; set; }
}
