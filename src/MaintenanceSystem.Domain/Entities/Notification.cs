using System;

namespace MaintenanceSystem.Domain.Entities;

public class Notification
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public string Message { get; init; } = string.Empty;
    public string Type { get; init; } = "Info";
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
