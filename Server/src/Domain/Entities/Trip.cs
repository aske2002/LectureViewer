namespace backend.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

public class Trip : BaseAuditableEntity<TripId>
{
    public DestinationId DestinationId { get; init; } = StronglyTypedId<DestinationId>.Default();
    public Destination Destination { get; init; } = null!;
    public required ClassYearId ClassYearId { get; init; } 
    public ClassYear ClassYear { get; init; } = null!;
    public string? Name { get; init; }
    public IList<TripDescription> DescriptionParts { get; init; } = new List<TripDescription>();
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
}