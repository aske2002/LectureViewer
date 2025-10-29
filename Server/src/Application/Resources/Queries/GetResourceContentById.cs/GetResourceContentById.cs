using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Domain.Identifiers;

namespace backend.Application.Users.Queries.GetUserInfo;

[Authorize]
public record GetResourceContentByIdQuery(ResourceId ResourceId) : IRequest<byte[]>;
public class GetResourceContentByIdQueryHandler : IRequestHandler<GetResourceContentByIdQuery, byte[]>
{
    private readonly IResourceFileManager _resourceFileManager;
    private readonly IMapper _mapper;

    public GetResourceContentByIdQueryHandler(IResourceFileManager resourceFileManager, IMapper mapper)
    {
        _resourceFileManager = resourceFileManager;
        _mapper = mapper;
    }

    public async Task<byte[]> Handle(GetResourceContentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _resourceFileManager.GetResourceByIdAsync(request.ResourceId);
    }
}
