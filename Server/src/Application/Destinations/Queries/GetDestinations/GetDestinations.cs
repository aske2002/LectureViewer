using backend.Application.Common.Models;
using backend.Application.Common.Security;
using backend.Application.Countries.Queries.GetCountries;
using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Destinations.Queries.GetDestinations;

[Authorize]
public record GetDestinationsQuery : FilteredQuery, IRequest<FilteredList<DestinationDto>>;

public class GetDestinationsQueryHandler : IRequestHandler<GetDestinationsQuery, FilteredList<DestinationDto>>
{
    private readonly IRepository<Destination, DestinationId> _repository;
    private readonly IMapper _mapper;

    public GetDestinationsQueryHandler(IRepository<Destination, DestinationId> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FilteredList<DestinationDto>> Handle(GetDestinationsQuery request, CancellationToken cancellationToken)
    {
        var response = await _repository.QueryAsync<DestinationDto>(include: q => q.Include(d => d.Country));
        response.Entities = response.Entities.Select(d => new DestinationDto
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            Country = _mapper.Map<CountryDto>(d.Country),
        })
            .ToList();

        return FilteredList<DestinationDto>.Create(response);

    }
}
