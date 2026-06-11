namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record CompleteChecklistStepCommand(Guid WorkOrderId, Guid StepId, bool IsCompleted);
