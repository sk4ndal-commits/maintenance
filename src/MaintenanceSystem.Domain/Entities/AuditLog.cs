namespace MaintenanceSystem.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public string? Details { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AuditLog() { }

    public static AuditLog Create(string action, Guid entityId, string? details = null) => new()
    {
        Id = Guid.NewGuid(),
        Action = action,
        EntityId = entityId,
        Details = details,
        CreatedAt = DateTime.UtcNow
    };
}
