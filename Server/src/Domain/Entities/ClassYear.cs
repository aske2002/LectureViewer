using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class ClassYear : BaseAuditableEntity<ClassYearId>
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public ICollection<Trip> Trips { get; init; } = new List<Trip>();
}