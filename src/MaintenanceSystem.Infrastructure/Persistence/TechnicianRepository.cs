using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class TechnicianRepository : ITechnicianRepository
{
    private readonly AppDbContext _context;

    public TechnicianRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Technician>> GetAllActiveAsync()
    {
        return await _context.Technicians
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Technician?> GetByIdAsync(Guid id)
    {
        return await _context.Technicians.FindAsync(id);
    }

    public async Task<Technician?> GetByEmailAsync(string email)
    {
        return await _context.Technicians.FirstOrDefaultAsync(t => t.Email == email);
    }

    public async Task<IEnumerable<Technician>> GetAllAsync()
    {
        return await _context.Technicians.OrderBy(t => t.Name).ToListAsync();
    }

    public async Task AddAsync(Technician technician)
    {
        await _context.Technicians.AddAsync(technician);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Technician technician)
    {
        _context.Technicians.Update(technician);
        await _context.SaveChangesAsync();
    }
}
