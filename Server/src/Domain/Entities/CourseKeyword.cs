using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class CourseKeyword : BaseAuditableEntity<CourseKeywordId>
{
    public required CourseId CourseId { get; set; }
    public required Course Course { get; set; } = null!;
    public required KeywordId KeywordId { get; set; }
    public required Keyword Keyword { get; set; } = null!;
}