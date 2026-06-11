using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record CreateWorkOrderCommand(
    Guid AssetId,
    string Title,
    WorkOrderPriority Priority,
    string? Description,
    DateTime? DueDate
);
