using backend.Application.Common.Interfaces;
using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.TodoLists.Commands.CreateTodoList;

public record CreateResourceCommand : IRequest<ResourceId>
{
    public Guid EntityId { get; init; } = Guid.Empty;
    public required string FileName { get; init; }
    public ResourceType ResourceType { get; init; }
    public required string MimeType { get; init; }
    public int Size { get; init; }
    public required byte[] Bytes { get; init; }
    public string? Url { get; init; }
    public int? Order { get; init; }
}

public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, ResourceId>
{
    private readonly IResourceService _resourceService;

    public CreateResourceCommandHandler(IApplicationDbContext context, IResourceService resourceService)
    {
        _resourceService = resourceService;
    }
    public async Task<ResourceId> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _resourceService.CreateResourceAsync(
            request.EntityId,
            request.FileName,
            request.ResourceType,
            request.Size,
            request.Bytes,
            request.MimeType,
            request.Order,
            cancellationToken
        );

        return resource.Id;
    }
}
