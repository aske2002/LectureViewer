namespace backend.Domain.Entities;

using System.Diagnostics.CodeAnalysis;
using backend.Domain.Identifiers;

[ExcludeFromCodeCoverage]
public class Country : BaseAuditableEntity<CountryId>
{
    public string? Name { get; init; }
    public string? IsoCode { get; init; }
    public string? Description { get; init; }
}