using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.DTOs;

public record WorkOrderDto(
    Guid WorkOrderId,
    Guid AssetId,
    string Title,
    WorkOrderStatus Status,
    string Priority,
    string? Description,
    DateTime CreatedAt,
    DateTime? CompletedAt
)
{
    public static WorkOrderDto From(WorkOrder w) => new(
        w.WorkOrderId, w.AssetId, w.Title, w.Status,
        w.Priority, w.Description, w.CreatedAt, w.CompletedAt);
}
