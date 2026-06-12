using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Users.Commands;

public class CreateUserHandler
{
    private readonly ITechnicianRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IAuditLogger _audit;

    public CreateUserHandler(ITechnicianRepository repo, IPasswordHasher hasher, IAuditLogger audit)
    {
        _repo = repo;
        _hasher = hasher;
        _audit = audit;
    }

    public async Task<Technician> HandleAsync(CreateUserRequest req)
    {
        var hash = _hasher.Hash(req.Password);
        var user = Technician.Create(req.Name, req.Email, hash, req.Role);
        await _repo.AddAsync(user);
        await _audit.LogAsync("User.Created", user.TechnicianId, $"Role={user.Role}");
        return user;
    }
}

public record CreateUserRequest(string Name, string Email, string Password, string Role);
