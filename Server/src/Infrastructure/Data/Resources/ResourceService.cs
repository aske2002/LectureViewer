
using backend.Application.Common.Exceptions;
using backend.Application.Common.Interfaces;
using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data.Resources;

public class ResourceService : IResourceService
{
    private readonly IResourceFileManager _resourceFileManager;
    private readonly ApplicationDbContext _context;
    public ResourceService(IResourceFileManager resourceFileManager, ApplicationDbContext context)
    {
        _resourceFileManager = resourceFileManager;
        _context = context;
    }

    public async Task<Resource> CreateResourceAsync(Guid entityId, string fileName, ResourceType resourceType, int size, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default)
    {
        var resource = new Resource
        {
            FileName = fileName,
            ResourceType = resourceType,
            Size = size,
            MimeType = mimeType ?? MimeTypesMap.GetMimeType(fileName) ?? "application/octet-stream",
            Order = order,
            EntityId = entityId
        };
        await _context.Resources.AddAsync(resource);
        await _context.SaveChangesAsync(cancellationToken);
        await _resourceFileManager.SaveResourceAync(resource.Id, bytes);

        return resource;
    }

    public async Task<Resource> CreateResourceAsync<TEntity, TId>(TEntity entity, string fileName, ResourceType resourceType, int size, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default) where TEntity : BaseEntity<TId> where TId : StronglyTypedId<TId>
    {
        return await CreateResourceAsync(entity.Id.Value, fileName, resourceType, size, bytes, mimeType, order, cancellationToken);
    }

    public async Task<Resource> GetResourceByIdAsync(ResourceId id, CancellationToken cancellationToken)
    {
        var resource = await _context.Resources
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return resource ?? throw new ResourceNotFoundException(id);
    }

    public async Task<ICollection<Resource>> GetResourcesByEntityAsync<TEntity, TId>(TEntity entity, ResourceType? resourceType, CancellationToken cancellationToken) where TEntity : BaseEntity<TId> where TId : StronglyTypedId<TId>
    {
        var entityId = entity.Id.Value;
        return await GetResourcesByEntityAsync(entityId, resourceType, cancellationToken);
    }

    public async Task<ICollection<Resource>> GetResourcesByEntityAsync(Guid EntityId, ResourceType? resourceType, CancellationToken cancellationToken)
    {
        var queryableResources = _ = _context.Resources
            .Where(x => x.EntityId == EntityId)
            .AsQueryable();

        if (resourceType.HasValue)
        {
            queryableResources = queryableResources.Where(x => x.ResourceType == resourceType.Value);
        }
        var resources = await queryableResources.ToListAsync(cancellationToken);
        return resources;
    }

    public Task<Resource?> GetResourceByEntityAsync(Guid id, ResourceType resourceType, CancellationToken cancellationToken)
    {
        var resource = _context.Resources
            .Where(x => x.EntityId == id && x.ResourceType == resourceType)
            .FirstOrDefaultAsync(cancellationToken);
        return resource;
    }

    public async Task<Resource?> GetResourceByEntityAsync<TEntity, TId>(TEntity entity, ResourceType resourceType, CancellationToken cancellationToken) where TEntity : BaseEntity<TId> where TId : StronglyTypedId<TId>
    {
        var entityId = entity.Id.Value;
        return await GetResourceByEntityAsync(entityId, resourceType, cancellationToken);
    }

    public async Task<IFormFile> GetResourceContentByIdAsync(ResourceId resourceId, CancellationToken cancellationToken)
    {
        var resource = await GetResourceByIdAsync(resourceId, cancellationToken);
        var content = await _resourceFileManager.GetResourceByIdAsync(resourceId);
        if (content == null)
        {
            throw new ResourceContentNotFoundException(resourceId);
        }

        return new FormFile(new MemoryStream(content), 0, content.Length, resource.FileName, resource.FileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = resource.MimeType,
        };
    }

}