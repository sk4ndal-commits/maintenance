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
    private readonly ChangeWorkOrderStatusHandler _statusHandler;
    private readonly AddChecklistStepHandler _addStepHandler;
    private readonly CompleteChecklistStepHandler _completeStepHandler;
    private readonly IWorkOrderRepository _repo;
    private readonly IAssignmentHistoryRepository _historyRepo;
    private readonly IChecklistStepRepository _stepsRepo;

    public WorkOrdersController(
        CreateWorkOrderHandler createHandler,
        AssignWorkOrderHandler assignHandler,
        ChangeWorkOrderStatusHandler statusHandler,
        AddChecklistStepHandler addStepHandler,
        CompleteChecklistStepHandler completeStepHandler,
        IWorkOrderRepository repo,
        IAssignmentHistoryRepository historyRepo,
        IChecklistStepRepository stepsRepo)
    {
        _createHandler = createHandler;
        _assignHandler = assignHandler;
        _statusHandler = statusHandler;
        _addStepHandler = addStepHandler;
        _completeStepHandler = completeStepHandler;
        _repo = repo;
        _historyRepo = historyRepo;
        _stepsRepo = stepsRepo;
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
        return Ok(new { data = items.Select(w => WorkOrderDto.From(w)), total, page, pageSize });
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

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeWorkOrderStatusCommand cmd)
    {
        if (id != cmd.WorkOrderId) return BadRequest(new { message = "ID mismatch" });
        try
        {
            var dto = await _statusHandler.Handle(cmd);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/assignment-history")]
    public async Task<IActionResult> GetAssignmentHistory(Guid id)
    {
        var history = await _historyRepo.GetByWorkOrderIdAsync(id);
        return Ok(history);
    }

    [HttpGet("{id:guid}/checklist")]
    public async Task<IActionResult> GetChecklist(Guid id)
    {
        var steps = await _stepsRepo.GetByWorkOrderIdAsync(id);
        return Ok(steps.Select(ChecklistStepDto.From));
    }

    [HttpPost("{id:guid}/checklist")]
    public async Task<IActionResult> AddStep(Guid id, [FromBody] AddChecklistStepCommand cmd)
    {
        if (id != cmd.WorkOrderId) return BadRequest(new { message = "ID mismatch" });
        try
        {
            var dto = await _addStepHandler.Handle(cmd);
            return CreatedAtAction(nameof(GetChecklist), new { id }, dto);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpPut("{id:guid}/checklist/{stepId:guid}")]
    public async Task<IActionResult> ToggleStep(Guid id, Guid stepId, [FromBody] CompleteChecklistStepCommand cmd)
    {
        if (id != cmd.WorkOrderId || stepId != cmd.StepId) return BadRequest(new { message = "ID mismatch" });
        try
        {
            var dto = await _completeStepHandler.Handle(cmd);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
    }
}
