using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.TodoLists.Commands.CreateTodoList;

public record CreateCountryCommand : IRequest<CountryId>
{
    public string? Name { get; init; }
    public string? IsoCode { get; init; }
    public string? Description { get; init; }
}

public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, CountryId>
{
    private readonly IApplicationDbContext _context;

    public CreateCountryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CountryId> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Country
        {
            Name = request.Name,
            IsoCode = request.IsoCode,
            Description = request.Description,
        };

        _context.Countries.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
