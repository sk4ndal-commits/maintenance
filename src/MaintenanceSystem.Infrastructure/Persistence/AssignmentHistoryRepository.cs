using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class AssignmentHistoryRepository : IAssignmentHistoryRepository
{
    private readonly AppDbContext _context;

    public AssignmentHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AssignmentHistory history)
    {
        await _context.AssignmentHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AssignmentHistory>> GetByWorkOrderIdAsync(Guid workOrderId)
    {
        return await _context.AssignmentHistories
            .Where(h => h.WorkOrderId == workOrderId)
            .OrderByDescending(h => h.AssignedAt)
            .ToListAsync();
    }
}
