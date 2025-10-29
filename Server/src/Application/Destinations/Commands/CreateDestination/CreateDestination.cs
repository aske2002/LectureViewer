using backend.Application.Common.Exceptions;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Destinations.Commands.CreateDestination;

public record CreateDestinationCommand : IRequest<DestinationId>
{
    public required string Name { get; init; }
    public required CountryId CountryId { get; init; }
    public string? Description { get; init; }
}

public class CreateDestinationCommandHandler : IRequestHandler<CreateDestinationCommand, DestinationId>
{
    private readonly IRepository<Country, CountryId> _countryRepository;
    private readonly IRepository<Destination, DestinationId> _destinationRepository;

    public CreateDestinationCommandHandler(IRepository<Country, CountryId> countryRepository,
        IRepository<Destination, DestinationId> destinationRepository)
    {
        _countryRepository = countryRepository;
        _destinationRepository = destinationRepository;
    }

    public async Task<DestinationId> Handle(CreateDestinationCommand request, CancellationToken cancellationToken)
    {

        var country = await _countryRepository.GetByIdAsync(request.CountryId);

        if (country == null)
        {
            throw new CountryNotFoundException(request.CountryId);
        }

        var destination = new Destination
        {
            Name = request.Name,
            Country = country,
            Description = request.Description
        };

        return await _destinationRepository.AddAsync(destination);

    }
}
