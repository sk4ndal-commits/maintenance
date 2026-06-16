using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly AppDbContext _db;

    public PasswordResetTokenRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(PasswordResetToken token)
    {
        await _db.PasswordResetTokens.AddAsync(token);
    }

    public async Task<PasswordResetToken?> GetByHashAsync(string tokenHash)
    {
        return await _db.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
    }

    public async Task UpdateAsync(PasswordResetToken token)
    {
        _db.PasswordResetTokens.Update(token);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
