using backend.Application.Countries.Queries.GetCountries;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Destinations.Queries.GetDestinations;

public record DestinationDto : BaseResponse<DestinationId>
{
    public string? Name { get; init; }
    public CountryDto Country { get; init; } = null!;
    public string? Description { get; init; }
}
