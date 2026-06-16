using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IPasswordResetTokenRepository
{
    Task AddAsync(PasswordResetToken token);
    Task<PasswordResetToken?> GetByHashAsync(string tokenHash);
    Task UpdateAsync(PasswordResetToken token);
    Task SaveChangesAsync();
}
