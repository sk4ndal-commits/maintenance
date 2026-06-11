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
    private readonly AssignWorkOrderHandler _assignHandler;
    private readonly IWorkOrderRepository _repo;
    private readonly IAssignmentHistoryRepository _historyRepo;

    public WorkOrdersController(
        CreateWorkOrderHandler createHandler,
        AssignWorkOrderHandler assignHandler,
        IWorkOrderRepository repo,
        IAssignmentHistoryRepository historyRepo)
    {
        _createHandler = createHandler;
        _assignHandler = assignHandler;
        _repo = repo;
        _historyRepo = historyRepo;
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
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? technicianId = null)
    {
        var (items, total) = await _repo.GetAllAsync(page, pageSize, technicianId);
        return Ok(new { data = items.Select(WorkOrderDto.From), total, page, pageSize });
    }

    [HttpPut("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignWorkOrderCommand cmd)
    {
        if (id != cmd.WorkOrderId) return BadRequest(new { message = "ID mismatch" });
        try
        {
            var dto = await _assignHandler.Handle(cmd);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/assignment-history")]
    public async Task<IActionResult> GetAssignmentHistory(Guid id)
    {
        var history = await _historyRepo.GetByWorkOrderIdAsync(id);
        return Ok(history);
    }
}
