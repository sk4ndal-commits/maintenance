using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IMaintenanceDocumentRepository
{
    Task AddAsync(MaintenanceDocument doc);
    Task<IEnumerable<MaintenanceDocument>> GetByWorkOrderIdAsync(Guid workOrderId);
    Task<MaintenanceDocument?> GetByIdAsync(Guid id);
}
