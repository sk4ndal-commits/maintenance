using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/technicians")]
public class TechniciansController : ControllerBase
{
    private readonly ITechnicianRepository _repo;

    public TechniciansController(ITechnicianRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var techs = await _repo.GetAllActiveAsync();
        return Ok(techs.Select(t => new { t.TechnicianId, t.Name, t.Email }));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTechnicianRequest req)
    {
        var tech = Technician.Create(req.Name, req.Email);
        await _repo.AddAsync(tech);
        return CreatedAtAction(nameof(GetAll), new { id = tech.TechnicianId },
            new { tech.TechnicianId, tech.Name, tech.Email });
    }
}

public record CreateTechnicianRequest(string Name, string Email);
