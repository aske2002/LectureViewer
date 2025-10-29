using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Lecture : BaseAuditableEntity<LectureId>
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public CourseId CourseId { get; init; } = CourseId.Default();
    public required Course Course { get; init; } = null!;
}