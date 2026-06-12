using MaintenanceSystem.Application.Common.Interfaces;

namespace MaintenanceSystem.Application.Users.Commands;

public class UpdateUserHandler
{
    private readonly ITechnicianRepository _repo;
    private readonly IAuditLogger _audit;

    public UpdateUserHandler(ITechnicianRepository repo, IAuditLogger audit)
    {
        _repo = repo;
        _audit = audit;
    }

    public async Task HandleAsync(Guid id, UpdateUserRequest req)
    {
        var user = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException($"User {id} not found");
        user.Update(req.Name, req.Email, req.Role);
        await _repo.UpdateAsync(user);
        await _audit.LogAsync("User.Updated", user.TechnicianId, $"Role={user.Role}");
    }
}

public record UpdateUserRequest(string Name, string Email, string Role);
