using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Common.Interfaces;

public interface IResourceService
{
    Task<Resource> CreateResourceAsync(
        string fileName,
        ResourceType resourceType,
        byte[] bytes,
        string? mimeType = null,
        int? order = null,
        CancellationToken cancellationToken = default
    );
    Task<Resource> CreateResourceAsync(
        IFormFile file,
        ResourceType resourceType,
        int? order = null,
        CancellationToken cancellationToken = default
    );

    Task<Resource> AddAssociatedResourceAsync(
        ResourceId parentResourceId,
        string fileName,
        ResourceType resourceType,
        byte[] bytes,
        string? mimeType = null,
        int? order = null,
        CancellationToken cancellationToken = default
    );

    Task<Stream> GetResourceStreamByIdAsync(ResourceId resourceId, CancellationToken cancellationToken);
    Task<IFormFile> GetResourceContentByIdAsync(ResourceId resourceId, CancellationToken cancellationToken);
    Task<Resource> GetResourceByIdAsync(ResourceId id, CancellationToken cancellationToken);
}