using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class CompleteWorkOrderHandler
{
    private readonly IWorkOrderRepository _repo;
    private readonly IChecklistStepRepository _stepsRepo;
    private readonly IAuditLogger _audit;

    public CompleteWorkOrderHandler(
        IWorkOrderRepository repo,
        IChecklistStepRepository stepsRepo,
        IAuditLogger audit)
    {
        _repo = repo;
        _stepsRepo = stepsRepo;
        _audit = audit;
    }

    public async Task<WorkOrderDto> Handle(CompleteWorkOrderCommand cmd)
    {
        var wo = await _repo.GetByIdAsync(cmd.WorkOrderId)
            ?? throw new KeyNotFoundException("WorkOrder not found");

        var steps = await _stepsRepo.GetByWorkOrderIdAsync(wo.WorkOrderId);
        wo.Complete(steps, cmd.CompletionNotes);

        await _repo.UpdateAsync(wo);
        await _audit.LogAsync("WorkOrder.Completed", wo.WorkOrderId,
            $"CompletedAt={wo.CompletedAt:O}; Notes={cmd.CompletionNotes ?? "(none)"}");

        return WorkOrderDto.From(wo, steps);
    }
}
