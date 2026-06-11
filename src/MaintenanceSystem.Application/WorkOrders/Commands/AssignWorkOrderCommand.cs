namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record AssignWorkOrderCommand(Guid WorkOrderId, Guid TechnicianId);
