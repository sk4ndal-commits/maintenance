using MaintenanceSystem.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MaintenanceSystem.Infrastructure.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration config)
    {
        _basePath = config["FileStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> StoreAsync(string fileName, string contentType, Stream content)
    {
        var key = $"{Guid.NewGuid()}_{fileName}";
        var fullPath = Path.Combine(_basePath, key);
        await using var fs = File.Create(fullPath);
        await content.CopyToAsync(fs);
        return key;
    }

    public Task<Stream> RetrieveAsync(string storagePath)
    {
        var fullPath = Path.Combine(_basePath, storagePath);
        return Task.FromResult<Stream>(File.OpenRead(fullPath));
    }

    public Task DeleteAsync(string storagePath)
    {
        var fullPath = Path.Combine(_basePath, storagePath);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }
}
