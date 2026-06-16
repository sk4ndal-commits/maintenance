namespace MaintenanceSystem.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public string Token { get; init; } = string.Empty; // Store hashed
    public DateTime ExpiresAt { get; init; }
    public bool IsRevoked { get; set; }
}
