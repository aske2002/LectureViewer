using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Course : BaseAuditableEntity<CourseId>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string InternalIdentifier { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public SemesterId SemesterId { get; init; } = SemesterId.Default();
    public Semester Semester { get; init; } = null!;
    public ICollection<Lecture> Lectures { get; init; } = new List<Lecture>();
    public ICollection<ApplicationUser> Instructors { get; init; } = new List<ApplicationUser>();
    public ICollection<CourseEnrollment> Enrollments { get; init; } = new List<CourseEnrollment>();
    public ICollection<CourseInviteLink> InviteLinks { get; init; } = new List<CourseInviteLink>();
}