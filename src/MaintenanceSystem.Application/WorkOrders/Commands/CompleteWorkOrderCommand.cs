namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record CompleteWorkOrderCommand(Guid WorkOrderId, string? CompletionNotes);
