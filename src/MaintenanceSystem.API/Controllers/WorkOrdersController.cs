using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.Commands;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/work-orders")]
public class WorkOrdersController : ControllerBase
{
    private readonly CreateWorkOrderHandler _createHandler;
    private readonly IWorkOrderRepository _repo;

    public WorkOrdersController(CreateWorkOrderHandler createHandler, IWorkOrderRepository repo)
    {
        _createHandler = createHandler;
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkOrderCommand cmd)
    {
        try
        {
            var dto = await _createHandler.Handle(cmd);
            return CreatedAtAction(nameof(GetById), new { id = dto.WorkOrderId }, dto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var wo = await _repo.GetByIdAsync(id);
        if (wo is null) return NotFound();
        return Ok(WorkOrderDto.From(wo));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (items, total) = await _repo.GetAllAsync(page, pageSize);
        return Ok(new { data = items.Select(WorkOrderDto.From), total, page, pageSize });
    }
}
