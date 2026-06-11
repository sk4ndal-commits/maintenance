using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Assets.DTOs;

public record AssetDto(
    Guid AssetId,
    string Name,
    string Type,
    string Location,
    string? Description,
    string QrCodePayload,
    DateTime CreatedAt
)
{
    public static AssetDto From(Asset a) => new(
        a.AssetId,
        a.Name,
        a.Type,
        a.Location,
        a.Description,
        a.QrCodePayload,
        a.CreatedAt
    );
}
