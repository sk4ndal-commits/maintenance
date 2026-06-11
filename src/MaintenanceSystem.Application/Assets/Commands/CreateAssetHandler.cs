using MaintenanceSystem.Application.Assets.DTOs;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Assets.Commands;

public class CreateAssetHandler
{
    private readonly IAssetRepository _repo;
    private readonly IAuditLogger _audit;

    public CreateAssetHandler(IAssetRepository repo, IAuditLogger audit)
    {
        _repo = repo;
        _audit = audit;
    }

    public async Task<AssetDto> Handle(CreateAssetCommand cmd)
    {
        var asset = Asset.Create(cmd.Name, cmd.Type, cmd.Location, cmd.Description);
        await _repo.AddAsync(asset);
        await _audit.LogAsync("Asset.Created", asset.AssetId);
        return AssetDto.From(asset);
    }
}
