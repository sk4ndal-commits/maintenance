using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IChecklistStepRepository
{
    Task<IEnumerable<ChecklistStep>> GetByWorkOrderIdAsync(Guid workOrderId);
    Task AddAsync(ChecklistStep step);
    Task<ChecklistStep?> GetByIdAsync(Guid id);
    Task UpdateAsync(ChecklistStep step);
}
