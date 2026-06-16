namespace MaintenanceSystem.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public string? UserId { get; private set; }
    public string? Details { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AuditLog() { }

    public static AuditLog Create(string action, Guid entityId, string? userId, string? details = null) => new()
    {
        Id = Guid.NewGuid(),
        Action = action,
        EntityId = entityId,
        UserId = userId,
        Details = details,
        CreatedAt = DateTime.UtcNow
    };

    public static AuditLog Create(string action, string? userId, string? details = null) => new()
    {
        Id = Guid.NewGuid(),
        Action = action,
        EntityId = Guid.Empty,
        UserId = userId,
        Details = details,
        CreatedAt = DateTime.UtcNow
    };
}
