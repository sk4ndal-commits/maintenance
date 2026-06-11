using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class CreateWorkOrderHandler
{
    private readonly IWorkOrderRepository _repo;
    private readonly IAssetRepository _assetRepo;
    private readonly IAuditLogger _audit;

    public CreateWorkOrderHandler(IWorkOrderRepository repo, IAssetRepository assetRepo, IAuditLogger audit)
    {
        _repo = repo;
        _assetRepo = assetRepo;
        _audit = audit;
    }

    public async Task<WorkOrderDto> Handle(CreateWorkOrderCommand cmd)
    {
        var asset = await _assetRepo.GetByIdAsync(cmd.AssetId);
        if (asset is null) throw new KeyNotFoundException("Asset not found");

        var wo = WorkOrder.Create(cmd.AssetId, cmd.Title, cmd.Priority, cmd.Description, cmd.DueDate);
        await _repo.AddAsync(wo);
        await _audit.LogAsync("WorkOrder.Created", wo.WorkOrderId);
        return WorkOrderDto.From(wo);
    }
}
