using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Semester : BaseAuditableEntity<SemesterId>
{
    public ICollection<Course> Courses { get; init; } = new List<Course>();
    public required Season Season { get; init; }
    public required int Year { get; init; }
}