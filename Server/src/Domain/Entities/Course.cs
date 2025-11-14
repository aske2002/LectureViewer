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
    public IList<Lecture> Lectures { get; set; } = new List<Lecture>();
    public Colour? Colour { get; set; } = Colour.Blue;
    public IList<CourseInstructor> Instructors { get; set; } = new List<CourseInstructor>();
    public IList<CourseEnrollment> Enrollments { get; set; } = new List<CourseEnrollment>();
    public IList<CourseInviteLink> InviteLinks { get; set; } = new List<CourseInviteLink>();
}