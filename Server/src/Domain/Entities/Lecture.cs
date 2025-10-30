using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Lecture : BaseAuditableEntity<LectureId>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public CourseId CourseId { get; init; } = CourseId.Default();
    public required Course Course { get; init; } = null!;
}