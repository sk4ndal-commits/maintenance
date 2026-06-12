namespace MaintenanceSystem.Domain.Entities;

public class Technician
{
    public Guid TechnicianId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "Technician";
    public bool IsActive { get; private set; }

    private Technician() { }

    public static Technician Create(string name, string email, string passwordHash, string role = "Technician") => new()
    {
        TechnicianId = Guid.NewGuid(),
        Name = name,
        Email = email,
        PasswordHash = passwordHash,
        Role = role,
        IsActive = true
    };

    public void Update(string name, string email, string role)
    {
        Name = name;
        Email = email;
        Role = role;
    }

    public void SetPassword(string passwordHash) => PasswordHash = passwordHash;
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
