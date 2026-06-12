using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface ITechnicianRepository
{
    Task<IEnumerable<Technician>> GetAllAsync();
    Task<IEnumerable<Technician>> GetAllActiveAsync();
    Task<Technician?> GetByIdAsync(Guid id);
    Task<Technician?> GetByEmailAsync(string email);
    Task AddAsync(Technician technician);
    Task UpdateAsync(Technician technician);
}
