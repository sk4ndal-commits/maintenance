namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IAuditLogger
{
    Task LogAsync(string action, Guid entityId, string? details = null);
}
