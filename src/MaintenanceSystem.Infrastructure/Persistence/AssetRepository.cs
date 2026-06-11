using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _db;

    public AssetRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Asset?> GetByIdAsync(Guid id) =>
        await _db.Assets.FindAsync(id);

    public async Task<(IEnumerable<Asset> Items, int Total)> GetAllAsync(int page, int pageSize)
    {
        var total = await _db.Assets.CountAsync();
        var items = await _db.Assets
            .OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }

    public async Task AddAsync(Asset asset)
    {
        await _db.Assets.AddAsync(asset);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Asset asset)
    {
        _db.Assets.Update(asset);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset is not null)
        {
            _db.Assets.Remove(asset);
            await _db.SaveChangesAsync();
        }
    }
}
