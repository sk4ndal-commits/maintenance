using MaintenanceSystem.Application.Common.Interfaces;

namespace MaintenanceSystem.Application.Users.Commands;

public class SetUserActiveHandler
{
    private readonly ITechnicianRepository _repo;
    private readonly IAuditLogger _audit;

    public SetUserActiveHandler(ITechnicianRepository repo, IAuditLogger audit)
    {
        _repo = repo;
        _audit = audit;
    }

    public async Task HandleAsync(Guid id, bool active)
    {
        var user = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException($"User {id} not found");
        if (active) user.Activate(); else user.Deactivate();
        await _repo.UpdateAsync(user);
        await _audit.LogAsync(active ? "User.Activated" : "User.Deactivated", user.TechnicianId);
    }
}
