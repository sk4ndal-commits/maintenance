namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record AddChecklistStepCommand(Guid WorkOrderId, string Label, bool IsMandatory, bool RequiresPhoto = false);
