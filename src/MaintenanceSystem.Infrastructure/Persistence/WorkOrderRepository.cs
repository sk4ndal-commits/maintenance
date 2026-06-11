using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly AppDbContext _context;

    public WorkOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkOrder>> GetByAssetIdAsync(Guid assetId, int limit = 10)
    {
        return await _context.WorkOrders
            .Where(w => w.AssetId == assetId)
            .OrderByDescending(w => w.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }
}
