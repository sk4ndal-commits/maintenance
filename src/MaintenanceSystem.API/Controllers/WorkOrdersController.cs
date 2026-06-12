using System.Security.Claims;
using MaintenanceSystem.Application.Common;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.Commands;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/work-orders")]
[Authorize]
public class WorkOrdersController : ControllerBase
{
    private readonly CreateWorkOrderHandler _createHandler;
    private readonly AssignWorkOrderHandler _assignHandler;
    private readonly ChangeWorkOrderStatusHandler _statusHandler;
    private readonly AddChecklistStepHandler _addStepHandler;
    private readonly CompleteChecklistStepHandler _completeStepHandler;
    private readonly CompleteWorkOrderHandler _completeHandler;
    private readonly UploadMaintenanceDocumentHandler _uploadDocHandler;
    private readonly IWorkOrderRepository _repo;
    private readonly IAssignmentHistoryRepository _historyRepo;
    private readonly IChecklistStepRepository _stepsRepo;
    private readonly IMaintenanceDocumentRepository _docRepo;
    private readonly IFileStorageService _storage;

    public WorkOrdersController(
        CreateWorkOrderHandler createHandler,
        AssignWorkOrderHandler assignHandler,
        ChangeWorkOrderStatusHandler statusHandler,
        AddChecklistStepHandler addStepHandler,
        CompleteChecklistStepHandler completeStepHandler,
        CompleteWorkOrderHandler completeHandler,
        UploadMaintenanceDocumentHandler uploadDocHandler,
        IWorkOrderRepository repo,
        IAssignmentHistoryRepository historyRepo,
        IChecklistStepRepository stepsRepo,
        IMaintenanceDocumentRepository docRepo,
        IFileStorageService storage)
    {
        _createHandler = createHandler;
        _assignHandler = assignHandler;
        _statusHandler = statusHandler;
        _addStepHandler = addStepHandler;
        _completeStepHandler = completeStepHandler;
        _completeHandler = completeHandler;
        _uploadDocHandler = uploadDocHandler;
        _repo = repo;
        _historyRepo = historyRepo;
        _stepsRepo = stepsRepo;
        _docRepo = docRepo;
        _storage = storage;
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Planner}")]
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
    [Authorize(Roles = $"{Roles.Admin},{Roles.Planner}")]
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
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role == Roles.Technician)
        {
            var techIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(techIdStr, out var techId)) return Forbid();
            var wo = await _repo.GetByIdAsync(id);
            if (wo?.AssignedTechnicianId != techId) return Forbid();
        }
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
    [Authorize(Roles = $"{Roles.Admin},{Roles.Planner}")]
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

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteWorkOrderCommand cmd)
    {
        if (id != cmd.WorkOrderId) return BadRequest(new { message = "ID mismatch" });
        try
        {
            var dto = await _completeHandler.Handle(cmd);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
    }

    [HttpPost("{id:guid}/documents")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument(
        Guid id,
        IFormFile file,
        [FromForm] string? notes = null,
        [FromForm] string uploadedBy = "system")
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided" });

        var cmd = new UploadMaintenanceDocumentCommand(
            id, file.FileName, file.ContentType,
            file.OpenReadStream(), file.Length, notes, uploadedBy);
        try
        {
            var dto = await _uploadDocHandler.Handle(cmd);
            return CreatedAtAction(nameof(GetDocuments), new { id }, dto);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("{id:guid}/documents")]
    public async Task<IActionResult> GetDocuments(Guid id)
    {
        var docs = await _docRepo.GetByWorkOrderIdAsync(id);
        return Ok(docs.Select(d => MaintenanceDocumentDto.From(d)));
    }

    [HttpGet("{id:guid}/documents/{docId:guid}/download")]
    public async Task<IActionResult> DownloadDocument(Guid id, Guid docId)
    {
        var doc = await _docRepo.GetByIdAsync(docId);
        if (doc == null || doc.WorkOrderId != id) return NotFound();
        var stream = await _storage.RetrieveAsync(doc.StoragePath);
        return File(stream, doc.ContentType, doc.FileName);
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
