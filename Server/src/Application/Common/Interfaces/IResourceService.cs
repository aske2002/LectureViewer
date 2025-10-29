using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Common.Interfaces;

public interface IResourceService
{
    Task<ICollection<Resource>> GetResourcesByEntityAsync<TEntity, TId>(TEntity entity, ResourceType? resourceType, CancellationToken cancellationToken) where TEntity : BaseEntity<TId> where TId : StronglyTypedId<TId>;
    Task<ICollection<Resource>> GetResourcesByEntityAsync(Guid EntityId, ResourceType? resourceType, CancellationToken cancellationToken);
    Task<Resource?> GetResourceByEntityAsync(Guid id, ResourceType resourceType, CancellationToken cancellationToken);
    Task<Resource?> GetResourceByEntityAsync<TEntity, TId>(TEntity entity, ResourceType resourceType, CancellationToken cancellationToken) where TEntity : BaseEntity<TId> where TId : StronglyTypedId<TId>;
    Task<Resource> CreateResourceAsync(Guid entityId, string fileName, ResourceType resourceType, int size, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default);
    Task<Resource> CreateResourceAsync<TEntity, TId>(
        TEntity entity,
        string fileName,
        ResourceType resourceType,
        int size,
        byte[] bytes,
        string? mimeType = null,
        int? order = null,
        CancellationToken cancellationToken = default
    ) where TEntity : BaseEntity<TId>
        where TId : StronglyTypedId<TId>;

    Task<IFormFile> GetResourceContentByIdAsync(ResourceId resourceId, CancellationToken cancellationToken);
    Task<Resource> GetResourceByIdAsync(ResourceId id, CancellationToken cancellationToken);
}