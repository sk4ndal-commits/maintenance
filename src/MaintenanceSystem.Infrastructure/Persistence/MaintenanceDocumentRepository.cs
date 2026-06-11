using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class MaintenanceDocumentRepository : IMaintenanceDocumentRepository
{
    private readonly AppDbContext _db;

    public MaintenanceDocumentRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(MaintenanceDocument doc)
    {
        _db.MaintenanceDocuments.Add(doc);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<MaintenanceDocument>> GetByWorkOrderIdAsync(Guid workOrderId) =>
        await _db.MaintenanceDocuments.Where(d => d.WorkOrderId == workOrderId).ToListAsync();

    public async Task<MaintenanceDocument?> GetByIdAsync(Guid id) =>
        await _db.MaintenanceDocuments.FindAsync(id);
}
