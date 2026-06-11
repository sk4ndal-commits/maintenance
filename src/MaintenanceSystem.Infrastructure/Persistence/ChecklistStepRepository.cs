using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class ChecklistStepRepository : IChecklistStepRepository
{
    private readonly AppDbContext _db;

    public ChecklistStepRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<ChecklistStep>> GetByWorkOrderIdAsync(Guid workOrderId) =>
        await _db.ChecklistSteps.Where(s => s.WorkOrderId == workOrderId).ToListAsync();

    public async Task AddAsync(ChecklistStep step)
    {
        _db.ChecklistSteps.Add(step);
        await _db.SaveChangesAsync();
    }

    public async Task<ChecklistStep?> GetByIdAsync(Guid id) =>
        await _db.ChecklistSteps.FindAsync(id);

    public async Task UpdateAsync(ChecklistStep step)
    {
        _db.ChecklistSteps.Update(step);
        await _db.SaveChangesAsync();
    }
}
