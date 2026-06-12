using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(Technician technician);
}
