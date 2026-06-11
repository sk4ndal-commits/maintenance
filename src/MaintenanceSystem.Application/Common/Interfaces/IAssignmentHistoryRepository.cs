using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IAssignmentHistoryRepository
{
    Task AddAsync(AssignmentHistory history);
    Task<IEnumerable<AssignmentHistory>> GetByWorkOrderIdAsync(Guid workOrderId);
}
