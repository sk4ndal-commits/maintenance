using MaintenanceSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/audit")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly IAuditLogRepository _repo;

    public AuditController(IAuditLogRepository repo) => _repo = repo;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1) 
        => Ok(await _repo.GetAllAsync(page));

    [HttpGet("entity/{entityId:guid}")]
    public async Task<IActionResult> GetByEntity(Guid entityId) 
        => Ok(await _repo.GetByEntityIdAsync(entityId));
}
