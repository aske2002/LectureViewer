using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class CourseInstructor : BaseAuditableEntity<CourseInstructorId>
{
    public CourseId CourseId { get; init; } = CourseId.Default();
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; init; } = null!;

    public string InstructorId { get; init; } = string.Empty;
    [ForeignKey(nameof(InstructorId))]
    public ApplicationUser Instructor { get; init; } = null!;
}