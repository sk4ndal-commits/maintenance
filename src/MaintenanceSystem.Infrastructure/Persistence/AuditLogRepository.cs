using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _db;
    public AuditLogRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<AuditLog>> GetByEntityIdAsync(Guid entityId) =>
        await _db.AuditLogs.Where(a => a.EntityId == entityId).OrderByDescending(a => a.CreatedAt).ToListAsync();

    public async Task<IEnumerable<AuditLog>> GetAllAsync(int page = 1, int pageSize = 50) =>
        await _db.AuditLogs
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
}
