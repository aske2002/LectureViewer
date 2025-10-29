using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Course : BaseAuditableEntity<CourseId>
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public required string InternalIdentifier { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public ICollection<Lecture> Lectures { get; init; } = new List<Lecture>();
    public ICollection<ApplicationUser> Instructors { get; init; } = new List<ApplicationUser>();
    public ICollection<CourseEnrollment> Enrollments { get; init; } = new List<CourseEnrollment>();
    public ICollection<CourseInviteLink> InviteLinks { get; init; } = new List<CourseInviteLink>();
}