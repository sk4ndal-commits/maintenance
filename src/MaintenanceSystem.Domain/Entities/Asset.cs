namespace MaintenanceSystem.Domain.Entities;

public class Asset
{
    public Guid AssetId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string QrCodePayload { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Asset() { }

    public static Asset Create(string name, string type, string location, string? description = null)
    {
        var id = Guid.NewGuid();
        return new Asset
        {
            AssetId = id,
            Name = name,
            Type = type,
            Location = location,
            Description = description,
            QrCodePayload = $"asset:{id}",
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string type, string location, string? description)
    {
        Name = name;
        Type = type;
        Location = location;
        Description = description;
    }
}
