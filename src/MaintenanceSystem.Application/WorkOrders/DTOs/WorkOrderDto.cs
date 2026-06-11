using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.DTOs;

public record WorkOrderDto(
    Guid WorkOrderId,
    Guid AssetId,
    string Title,
    WorkOrderStatus Status,
    WorkOrderPriority Priority,
    string? Description,
    DateTime? DueDate,
    Guid? AssignedTechnicianId,
    string? AssignedTechnicianName,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    IEnumerable<ChecklistStepDto> ChecklistSteps
)
{
    public static WorkOrderDto From(WorkOrder w, IEnumerable<ChecklistStep>? steps = null) => new(
        w.WorkOrderId, w.AssetId, w.Title, w.Status,
        w.Priority, w.Description, w.DueDate,
        w.AssignedTechnicianId, w.AssignedTechnicianName,
        w.CreatedAt, w.CompletedAt,
        (steps ?? []).Select(ChecklistStepDto.From));
}
