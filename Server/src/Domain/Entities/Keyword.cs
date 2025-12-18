using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Keyword : BaseAuditableEntity<KeywordId>
{
    private string _text = string.Empty;
    public string Text
    {
        get => _text;
        set => _text = value.ToLower();
    }
    public IList<CourseKeyword> CourseKeywords { get; private set; } = new List<CourseKeyword>();
    public IList<KeywordExtractionMediaProcessingJob> SourceJobs { get; private set; } = new List<KeywordExtractionMediaProcessingJob>();
    public IList<TranscriptKeywordOccurrence> TranscriptOccurrences { get; private set; } = new List<TranscriptKeywordOccurrence>();
    public IList<DocumentKeyword> DocumentOccurrences { get; private set; } = new List<DocumentKeyword>();
}