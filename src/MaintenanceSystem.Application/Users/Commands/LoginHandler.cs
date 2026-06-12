using MaintenanceSystem.Application.Common.Interfaces;

namespace MaintenanceSystem.Application.Users.Commands;

public class LoginHandler
{
    private readonly ITechnicianRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwt;

    public LoginHandler(ITechnicianRepository repo, IPasswordHasher hasher, IJwtService jwt)
    {
        _repo = repo;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<string?> HandleAsync(string email, string password)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user is null || !user.IsActive || !_hasher.Verify(password, user.PasswordHash))
            return null;
        return _jwt.GenerateToken(user);
    }
}
