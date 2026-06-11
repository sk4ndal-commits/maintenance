using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IAssetRepository
{
    Task<Asset?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Asset> Items, int Total)> GetAllAsync(int page, int pageSize);
    Task AddAsync(Asset asset);
    Task UpdateAsync(Asset asset);
    Task DeleteAsync(Guid id);
}
