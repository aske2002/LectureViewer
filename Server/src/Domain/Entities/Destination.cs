namespace backend.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

public class Destination : BaseAuditableEntity<DestinationId>
{
    public string? Name { get; init; }
    public CountryId CountryId { get; init; } = CountryId.Default();
    public Country Country { get; init; } = null!;
    public string? Description { get; init; }
    public IList<Trip> Trips { get; init; } = new List<Trip>();
}