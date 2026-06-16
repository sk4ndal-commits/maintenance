namespace MaintenanceSystem.Domain.Entities;

public class PasswordResetToken
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public string TokenHash { get; init; } = string.Empty; // Gehasht speichern!
    public DateTime ExpiresAt { get; init; }
    public bool IsUsed { get; set; }
}
