using backend.Application.Common.Exceptions;
using backend.Application.Common.Interfaces;
using backend.Domain.Identifiers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace backend.Infrastructure.Data.Resources;

public class ResourceFileManager : IResourceFileManager
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    public ResourceFileManager(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }
    private string ResourceRoot => _configuration["ResourceRoot"] ?? Path.Combine(_environment.ContentRootPath, "Resources");
    private string GetAndEnsureResource(ResourceId resourceId)
    {
        var path = GetResourcePath(resourceId);

        if (!File.Exists(path))
        {
            throw new ResourceContentNotFoundException(resourceId);
        }


        return path;
    }

    private void EnsureValidData(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            throw new ResourceEmptyException();
        }
    }


    public async Task<string> SaveResourceAync(ResourceId id, byte[] data)
    {
        var path = GetResourcePath(id);
        EnsureValidData(data);

        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            await stream.WriteAsync(data, 0, data.Length);
        }

        return path;

    }

    public async Task<byte[]> GetResourceByIdAsync(ResourceId id)
    {
        var path = GetAndEnsureResource(id);

        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

    public string GetResourcePath(ResourceId id)
    {
        var path = Path.Combine(ResourceRoot, id.ToString());

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(ResourceRoot);
        }

        return Path.Combine(ResourceRoot, id.ToString());
    }

    public async Task ModifyResourceAsync(ResourceId id, byte[] data)
    {
        var path = GetAndEnsureResource(id);
        EnsureValidData(data);

        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            await stream.WriteAsync(data, 0, data.Length);
        }
    }

    public void DeleteResource(ResourceId id)
    {
        var path = GetAndEnsureResource(id);
        File.Delete(path);
    }
}
