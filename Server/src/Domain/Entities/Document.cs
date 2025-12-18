using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class DocumentPage : BaseAuditableEntity<DocumentPageId>
{
    public DocumentId DocumentId { get; set; } = DocumentId.Default();
    public Document Document { get; set; } = null!;
    public required int PageNumber { get; set; }
    public required string TextContent { get; set; }
}

public class Document : BaseAuditableEntity<DocumentId>
{
    public required ResourceId ResourceId { get; set; }
    public required Resource Resource { get; set; }
    public IList<DocumentPage> Pages { get; set; } = new List<DocumentPage>();
    public string? Title { get; set; }
    public string? Author { get; set; }
    public required int NumberOfPages { get; set; }
    public string? Summary { get; set; }
    public IList<DocumentKeyword> DocumentKeywords { get; private set; } = new List<DocumentKeyword>();
}

public class DocumentKeyword : BaseAuditableEntity<DocumentKeywordId>
{
    public required DocumentId DocumentId { get; set; }
    public required Document Document { get; set; }
    public required KeywordId KeywordId { get; set; }
    public required Keyword Keyword { get; set; }
    public required int PageNumber { get; set; }
    public required string SurroundingText { get; set; }
    public required int PositionX { get; set; }
    public required int PositionY { get; set; }
}