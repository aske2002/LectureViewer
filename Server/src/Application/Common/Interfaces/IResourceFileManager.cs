using backend.Application.Common.Models;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Common.Interfaces;

public interface IResourceFileManager
{
    Task ModifyResourceAsync(ResourceId id, byte[] data);
    void DeleteResource(ResourceId id);
    Task<byte[]> GetResourceByIdAsync(ResourceId id);
    Task<string> SaveResourceAync(ResourceId id, byte[] data);
    string GetResourcePath(ResourceId id);

}
