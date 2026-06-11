namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record CreateWorkOrderCommand(
    Guid AssetId,
    string Title,
    string Priority,
    string? Description
);
