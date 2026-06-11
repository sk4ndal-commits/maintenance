using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class AddChecklistStepHandler
{
    private readonly IChecklistStepRepository _repo;
    private readonly IWorkOrderRepository _woRepo;

    public AddChecklistStepHandler(IChecklistStepRepository repo, IWorkOrderRepository woRepo)
    {
        _repo = repo;
        _woRepo = woRepo;
    }

    public async Task<ChecklistStepDto> Handle(AddChecklistStepCommand cmd)
    {
        var wo = await _woRepo.GetByIdAsync(cmd.WorkOrderId)
            ?? throw new KeyNotFoundException("WorkOrder not found");

        var step = ChecklistStep.Create(wo.WorkOrderId, cmd.Label, cmd.IsMandatory, cmd.RequiresPhoto);
        await _repo.AddAsync(step);
        return ChecklistStepDto.From(step);
    }
}
