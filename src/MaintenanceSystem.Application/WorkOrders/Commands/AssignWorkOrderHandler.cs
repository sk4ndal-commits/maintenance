using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class AssignWorkOrderHandler
{
    private readonly IWorkOrderRepository _repo;
    private readonly ITechnicianRepository _techRepo;
    private readonly IAssignmentHistoryRepository _historyRepo;
    private readonly IAuditLogger _audit;

    public AssignWorkOrderHandler(
        IWorkOrderRepository repo,
        ITechnicianRepository techRepo,
        IAssignmentHistoryRepository historyRepo,
        IAuditLogger audit)
    {
        _repo = repo;
        _techRepo = techRepo;
        _historyRepo = historyRepo;
        _audit = audit;
    }

    public async Task<WorkOrderDto> Handle(AssignWorkOrderCommand cmd)
    {
        var wo = await _repo.GetByIdAsync(cmd.WorkOrderId)
            ?? throw new KeyNotFoundException("WorkOrder not found");
        var tech = await _techRepo.GetByIdAsync(cmd.TechnicianId)
            ?? throw new KeyNotFoundException("Technician not found");

        wo.Assign(tech.TechnicianId, tech.Name);
        await _repo.UpdateAsync(wo);

        var history = AssignmentHistory.Create(wo.WorkOrderId, tech.TechnicianId, tech.Name, "system");
        await _historyRepo.AddAsync(history);

        await _audit.LogAsync("WorkOrder.Assigned", wo.WorkOrderId);
        return WorkOrderDto.From(wo);
    }
}
