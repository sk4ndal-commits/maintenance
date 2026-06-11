using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IAuditLogRepository
{
    Task<IEnumerable<AuditLog>> GetByEntityIdAsync(Guid entityId);
}
