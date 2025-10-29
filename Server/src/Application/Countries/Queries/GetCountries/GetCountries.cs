using System;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Countries.Queries.GetCountries;

public record GetCountriesQuery : IRequest<List<CountryDto>>;

public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDto>>
{
    private readonly IRepository<Country, CountryId> _repository;

    public GetCountriesQueryHandler(IRepository<Country, CountryId> repository)
    {
        _repository = repository;
    }

    public async Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        return (await _repository.QueryAsync<CountryDto>(
            filter: null,
            orderBy: null,
            include: null)).Entities;
    }
}
