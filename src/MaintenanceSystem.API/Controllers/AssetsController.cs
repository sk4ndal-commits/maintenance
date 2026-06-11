using MaintenanceSystem.Application.Assets.Commands;
using MaintenanceSystem.Application.Assets.DTOs;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController : ControllerBase
{
    private readonly CreateAssetHandler _createHandler;
    private readonly UpdateAssetHandler _updateHandler;
    private readonly IAssetRepository _repo;
    private readonly IWorkOrderRepository _workOrderRepo;

    public AssetsController(CreateAssetHandler createHandler, UpdateAssetHandler updateHandler, IAssetRepository repo, IWorkOrderRepository workOrderRepo)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _repo = repo;
        _workOrderRepo = workOrderRepo;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssetCommand cmd)
    {
        var dto = await _createHandler.Handle(cmd);
        return CreatedAtAction(nameof(GetById), new { id = dto.AssetId }, dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var asset = await _repo.GetByIdAsync(id);
        if (asset is null) return NotFound();
        return Ok(AssetDto.From(asset));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (items, total) = await _repo.GetAllAsync(page, pageSize);
        return Ok(new
        {
            data = items.Select(AssetDto.From),
            total,
            page,
            pageSize
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetCommand cmd)
    {
        if (id != cmd.AssetId) return BadRequest("ID mismatch");
        var dto = await _updateHandler.Handle(cmd);
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    [HttpGet("{id:guid}/work-orders")]
    public async Task<IActionResult> GetWorkOrders(Guid id, [FromQuery] int limit = 10)
    {
        var asset = await _repo.GetByIdAsync(id);
        if (asset is null) return NotFound();
        var orders = await _workOrderRepo.GetByAssetIdAsync(id, limit);
        return Ok(orders.Select(WorkOrderDto.From));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var asset = await _repo.GetByIdAsync(id);
        if (asset is null) return NotFound();
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
