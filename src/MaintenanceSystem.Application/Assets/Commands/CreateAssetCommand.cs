namespace MaintenanceSystem.Application.Assets.Commands;

public record CreateAssetCommand(
    string Name,
    string Type,
    string Location,
    string? Description
);
