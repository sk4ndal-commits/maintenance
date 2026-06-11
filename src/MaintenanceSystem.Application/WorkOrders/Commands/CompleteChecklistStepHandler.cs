using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class CompleteChecklistStepHandler
{
    private readonly IChecklistStepRepository _repo;
    private readonly IAuditLogger _audit;

    public CompleteChecklistStepHandler(IChecklistStepRepository repo, IAuditLogger audit)
    {
        _repo = repo;
        _audit = audit;
    }

    public async Task<ChecklistStepDto> Handle(CompleteChecklistStepCommand cmd)
    {
        var step = await _repo.GetByIdAsync(cmd.StepId)
            ?? throw new KeyNotFoundException("Step not found");

        if (step.WorkOrderId != cmd.WorkOrderId)
            throw new InvalidOperationException("Step does not belong to this WorkOrder.");

        if (cmd.IsCompleted) step.Complete();
        else step.Uncomplete();

        await _repo.UpdateAsync(step);
        await _audit.LogAsync("ChecklistStep.Toggled", step.WorkOrderId, $"Step {step.Id}: {cmd.IsCompleted}");
        return ChecklistStepDto.From(step);
    }
}
