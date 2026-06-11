using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using MaintenanceSystem.Infrastructure.Persistence;

namespace MaintenanceSystem.Infrastructure.Services;

public class AuditLogger : IAuditLogger
{
    private readonly AppDbContext _db;

    public AuditLogger(AppDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(string action, Guid entityId, string? details = null)
    {
        var log = AuditLog.Create(action, entityId, details);
        await _db.AuditLogs.AddAsync(log);
        await _db.SaveChangesAsync();
    }
}
