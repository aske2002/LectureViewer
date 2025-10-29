using backend.Application.Common.Attributes;
using backend.Application.Common.Interfaces;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Countries.Queries.GetCountries;

public record CountryDto : BaseResponse<CountryId>
{
    public string? Name { get; set; }
    public string? Code { get; set; }

    [ResourceResponse(ResourceType.Flag)]
    public ResourceResponse? Flag { get; set; }
}
