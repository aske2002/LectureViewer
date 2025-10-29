using backend.Application.Trips.Queries.GetTrips;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Countries.Queries.GetCountries;

public record GetTripsQuery : IRequest<List<TripDto>>;

public class GetTripsQueryHandler : IRequestHandler<GetTripsQuery, List<TripDto>>
{
    private readonly IRepository<Trip, TripId> _repository;

    public GetTripsQueryHandler(IRepository<Trip, TripId> repository)
    {
        _repository = repository;
    }

    public async Task<List<TripDto>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
    {
        var response = await _repository.QueryAsync<TripDto>(include: t => t.Include(t => t.DescriptionParts).Include(t => t.Destination).Include(t => t.ClassYear), cancellationToken: cancellationToken);
        return response.Entities;
    }
}
