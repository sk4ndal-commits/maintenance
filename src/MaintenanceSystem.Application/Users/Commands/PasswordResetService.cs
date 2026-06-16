using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Users.Commands;

public class PasswordResetService
{
    private readonly ITechnicianRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IAuditLogger _audit;
    private readonly IPasswordResetTokenRepository _tokenRepo;

    public PasswordResetService(ITechnicianRepository repo, IPasswordHasher hasher, IAuditLogger audit, IPasswordResetTokenRepository tokenRepo)
    {
        _repo = repo;
        _hasher = hasher;
        _audit = audit;
        _tokenRepo = tokenRepo;
    }

    public async Task<string?> GenerateResetTokenAsync(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user is null) return null; // Should not reveal if email exists

        var token = Guid.NewGuid().ToString();
        var tokenHash = _hasher.Hash(token);

        var resetToken = new PasswordResetToken
        {
            UserId = user.TechnicianId,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30)
        };

        await _tokenRepo.AddAsync(resetToken);
        await _tokenRepo.SaveChangesAsync();

        return token; // Return raw token to be sent by email
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var tokenHash = _hasher.Hash(token);
        var resetToken = await _tokenRepo.GetByHashAsync(tokenHash);

        if (resetToken is null || resetToken.IsUsed || resetToken.ExpiresAt <= DateTime.UtcNow)
            return false;

        var user = await _repo.GetByIdAsync(resetToken.UserId);
        if (user is null) return false;

        user.SetPassword(_hasher.Hash(newPassword));
        resetToken.IsUsed = true;
        
        await _repo.UpdateAsync(user);
        await _tokenRepo.UpdateAsync(resetToken);
        await _tokenRepo.SaveChangesAsync();
        await _audit.LogAsync("User.PasswordReset", user.TechnicianId, "Self-service password reset");

        return true;
    }
}
