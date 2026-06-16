using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using MaintenanceSystem.Infrastructure.Persistence;

namespace MaintenanceSystem.Infrastructure.Services;

public class AuditLogger : IAuditLogger
{
    private readonly AppDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AuditLogger(AppDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task LogAsync(string action, Guid entityId, string? details = null)
    {
        var log = AuditLog.Create(action, entityId, _currentUser.UserId, details);
        await _db.AuditLogs.AddAsync(log);
        await _db.SaveChangesAsync();
    }

    public async Task LogAsync(string action, string? details = null)
    {
        var log = AuditLog.Create(action, _currentUser.UserId, details);
        await _db.AuditLogs.AddAsync(log);
        await _db.SaveChangesAsync();
    }
}
