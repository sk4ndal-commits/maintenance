using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record ChangeWorkOrderStatusCommand(Guid WorkOrderId, WorkOrderStatus NewStatus);
