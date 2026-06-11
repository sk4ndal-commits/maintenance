using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IWorkOrderRepository
{
    Task<IEnumerable<WorkOrder>> GetByAssetIdAsync(Guid assetId, int limit = 10);
    Task<WorkOrder?> GetByIdAsync(Guid workOrderId);
    Task<(IEnumerable<WorkOrder> Items, int Total)> GetAllAsync(int page, int pageSize, Guid? technicianId = null);
    Task AddAsync(WorkOrder workOrder);
    Task UpdateAsync(WorkOrder workOrder);
}
