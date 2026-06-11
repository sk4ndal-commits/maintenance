namespace MaintenanceSystem.Application.Assets.Commands;

public record UpdateAssetCommand(
    Guid AssetId,
    string Name,
    string Type,
    string Location,
    string? Description
);
