using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Users.Commands;

public class ResetPasswordHandler
{
    private readonly ITechnicianRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IAuditLogger _audit;

    public ResetPasswordHandler(ITechnicianRepository repo, IPasswordHasher hasher, IAuditLogger audit)
    {
        _repo = repo;
        _hasher = hasher;
        _audit = audit;
    }

    public async Task HandleAsync(Guid id, string newPassword)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null) throw new Exception("User not found");

        var hash = _hasher.Hash(newPassword);
        user.SetPassword(hash);
        
        await _repo.UpdateAsync(user);
        await _audit.LogAsync("User.PasswordReset", user.TechnicianId, "Admin initiated password reset");
    }
}
