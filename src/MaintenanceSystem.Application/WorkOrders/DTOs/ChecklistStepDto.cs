using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.DTOs;

public record ChecklistStepDto(
    Guid Id,
    Guid WorkOrderId,
    string Label,
    bool IsMandatory,
    bool IsCompleted,
    DateTime? CompletedAt
)
{
    public static ChecklistStepDto From(ChecklistStep s) => new(
        s.Id, s.WorkOrderId, s.Label, s.IsMandatory, s.IsCompleted, s.CompletedAt);
}
