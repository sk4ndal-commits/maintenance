using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _db;
    public AuditLogRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<AuditLog>> GetByEntityIdAsync(Guid entityId) =>
        await _db.AuditLogs.Where(a => a.EntityId == entityId).ToListAsync();
}
