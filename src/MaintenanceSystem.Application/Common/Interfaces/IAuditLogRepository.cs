using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IAuditLogRepository
{
    Task<IEnumerable<AuditLog>> GetByEntityIdAsync(Guid entityId);
    Task<IEnumerable<AuditLog>> GetAllAsync(int page = 1, int pageSize = 50);
}
