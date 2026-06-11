using MaintenanceSystem.Application.Assets.Commands;
using MaintenanceSystem.Application.Assets.DTOs;
using MaintenanceSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController : ControllerBase
{
    private readonly CreateAssetHandler _createHandler;
    private readonly IAssetRepository _repo;

    public AssetsController(CreateAssetHandler createHandler, IAssetRepository repo)
    {
        _createHandler = createHandler;
        _repo = repo;
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var asset = await _repo.GetByIdAsync(id);
        if (asset is null) return NotFound();
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
