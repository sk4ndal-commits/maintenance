using MaintenanceSystem.Application.Common.Interfaces;
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
}
