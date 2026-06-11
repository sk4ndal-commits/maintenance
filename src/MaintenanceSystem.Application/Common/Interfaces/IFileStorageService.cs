namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> StoreAsync(string fileName, string contentType, Stream content);
    Task<Stream> RetrieveAsync(string storagePath);
    Task DeleteAsync(string storagePath);
}
