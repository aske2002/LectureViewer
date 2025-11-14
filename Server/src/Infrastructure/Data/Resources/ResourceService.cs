
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

    public async Task<Resource> GetResourceByIdAsync(ResourceId id, CancellationToken cancellationToken)
    {
        var resource = await _context.Resources
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return resource ?? throw new ResourceNotFoundException(id);
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

    public async Task<Resource> AddAssociatedResourceAsync(ResourceId parentResourceId, string fileName, ResourceType resourceType, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default)
    {
        var parentResource = await GetResourceByIdAsync(parentResourceId, cancellationToken);

        var associatedResource = await CreateResourceAsync(fileName, resourceType, bytes, mimeType, order, cancellationToken);
        associatedResource.ParentResourceId = parentResource.Id;

        _context.Resources.Update(associatedResource);
        await _context.SaveChangesAsync(cancellationToken);

        return associatedResource;
    }
    public Task<Resource> CreateResourceAsync(IFormFile file, ResourceType resourceType, int? order = null, CancellationToken cancellationToken = default)
    {


        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();
        return CreateResourceAsync(file.FileName, resourceType, bytes, file.ContentType, order, cancellationToken);
    }

    public async Task<Resource> CreateResourceAsync(string fileName, ResourceType resourceType, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default)
    {
        var resource = new Resource
        {
            FileName = fileName,
            ResourceType = resourceType,
            Size = bytes.Length,
            MimeType = mimeType ?? MimeTypesMap.GetMimeType(fileName) ?? "application/octet-stream",
            Order = order,
        };
        await _context.Resources.AddAsync(resource);
        await _context.SaveChangesAsync(cancellationToken);
        await _resourceFileManager.SaveResourceAync(resource.Id, bytes);

        return resource;
    }
}