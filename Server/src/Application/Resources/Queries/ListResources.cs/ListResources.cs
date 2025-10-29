using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Application.Common.Models;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Resources.Queries.ListResources;

public record ListResourcesWithPaginationQuery : IRequest<FilteredList<ResourceResponse>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListResourcesWithPaginationQueryHandler : IRequestHandler<ListResourcesWithPaginationQuery, FilteredList<ResourceResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IRepository<Resource, ResourceId> _repository;

    public ListResourcesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Resource, ResourceId> repository)
    {
        _context = context;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<FilteredList<ResourceResponse>> Handle(ListResourcesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.QueryAsync<ResourceResponse>(
            orderBy: q => q.OrderBy(r => r.FileName),
            cancellationToken: cancellationToken,
            pagination: (request.PageNumber, request.PageSize));
        return  FilteredList<ResourceResponse>.Create(resources, orderBy: null);
    }
}
