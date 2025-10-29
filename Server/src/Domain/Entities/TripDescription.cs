using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class TripDescription : BaseAuditableEntity<TripDescriptionId>
{
    public TripId TripId { get; init; } = TripId.Default();
    public Trip Trip { get; init; } = null!;
    public string? Title { get; init; }
    public string? Content { get; init; }
}