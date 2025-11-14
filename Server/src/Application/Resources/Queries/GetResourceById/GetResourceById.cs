using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Identifiers;

namespace backend.Application.Users.Queries.GetUserInfo;

[Authorize]
public record GetResourceByIdQuery(ResourceId ResourceId) : IRequest<Resources.Queries.GetResourceById.ResourceResponse>;
public class GetResourceByIdQueryHandler : IRequestHandler<GetResourceByIdQuery, Resources.Queries.GetResourceById.ResourceResponse>
{
    private readonly IResourceService _resourceService;
    private readonly IMapper _mapper;

    public GetResourceByIdQueryHandler(IResourceService resourceService, IMapper mapper)
    {
        _resourceService = resourceService;
        _mapper = mapper;
    }

    public async Task<ResourceResponse> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
    {
        var resource = await _resourceService.GetResourceByIdAsync(
            request.ResourceId,
            cancellationToken
        );
        return _mapper.Map<Resources.Queries.GetResourceById.ResourceResponse>(resource);
    }
}
