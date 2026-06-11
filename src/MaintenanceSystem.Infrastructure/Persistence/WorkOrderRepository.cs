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

    public async Task<WorkOrder?> GetByIdAsync(Guid workOrderId)
    {
        return await _context.WorkOrders.FindAsync(workOrderId);
    }

    public async Task<(IEnumerable<WorkOrder> Items, int Total)> GetAllAsync(int page, int pageSize, Guid? technicianId = null)
    {
        var query = _context.WorkOrders.AsQueryable();
        if (technicianId.HasValue)
            query = query.Where(w => w.AssignedTechnicianId == technicianId.Value);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(w => w.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }

    public async Task UpdateAsync(WorkOrder workOrder)
    {
        _context.WorkOrders.Update(workOrder);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(WorkOrder workOrder)
    {
        await _context.WorkOrders.AddAsync(workOrder);
        await _context.SaveChangesAsync();
    }
}
