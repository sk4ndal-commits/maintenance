using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class ChangeWorkOrderStatusHandler
{
    private readonly IWorkOrderRepository _repo;
    private readonly IChecklistStepRepository _stepsRepo;
    private readonly IAuditLogger _audit;

    public ChangeWorkOrderStatusHandler(IWorkOrderRepository repo, IChecklistStepRepository stepsRepo, IAuditLogger audit)
    {
        _repo = repo;
        _stepsRepo = stepsRepo;
        _audit = audit;
    }

    public async Task<WorkOrderDto> Handle(ChangeWorkOrderStatusCommand cmd)
    {
        var wo = await _repo.GetByIdAsync(cmd.WorkOrderId)
            ?? throw new KeyNotFoundException("WorkOrder not found");

        var steps = await _stepsRepo.GetByWorkOrderIdAsync(wo.WorkOrderId);
        wo.Transition(cmd.NewStatus, steps);

        await _repo.UpdateAsync(wo);
        await _audit.LogAsync("WorkOrder.StatusChanged", wo.WorkOrderId, $"{cmd.NewStatus}");

        return WorkOrderDto.From(wo, steps);
    }
}
