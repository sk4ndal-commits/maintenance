namespace MaintenanceSystem.Domain.Entities;

public class Technician
{
    public Guid TechnicianId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Technician() { }

    public static Technician Create(string name, string email) => new()
    {
        TechnicianId = Guid.NewGuid(),
        Name = name,
        Email = email,
        IsActive = true
    };
}
