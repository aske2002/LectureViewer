
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class CountryNotFoundException : NotFoundExceptionBase<CountryId, CountryNotFoundException>
{
    public override string EntityName => nameof(Country);
    public CountryNotFoundException(CountryId countryId)
        : base(countryId) { }
}