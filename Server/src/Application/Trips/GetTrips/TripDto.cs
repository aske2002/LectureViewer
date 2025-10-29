using backend.Application.Common.Attributes;
using backend.Application.Common.Interfaces;
using backend.Application.Destinations.Queries.GetDestinations;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Trips.Queries.GetTrips;

public record TripDto : BaseResponse<TripId>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Content { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public DestinationDto Destination { get; init; } = null!;
}
