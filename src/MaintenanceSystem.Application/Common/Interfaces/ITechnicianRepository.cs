using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface ITechnicianRepository
{
    Task<IEnumerable<Technician>> GetAllActiveAsync();
    Task<Technician?> GetByIdAsync(Guid id);
    Task AddAsync(Technician technician);
}
