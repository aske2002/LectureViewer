using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class CourseInstructor : BaseAuditableEntity<CourseInstructorId>
{
    public CourseId CourseId { get; init; } = CourseId.Default();
    public Course Course { get; init; } = null!;
    public string InstructorId { get; init; } = string.Empty;
    public ApplicationUser Instructor { get; init; } = null!;
}