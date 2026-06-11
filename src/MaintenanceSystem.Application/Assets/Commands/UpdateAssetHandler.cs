using MaintenanceSystem.Application.Assets.DTOs;
using MaintenanceSystem.Application.Common.Interfaces;

namespace MaintenanceSystem.Application.Assets.Commands;

public class UpdateAssetHandler
{
    private readonly IAssetRepository _repo;
    private readonly IAuditLogger _audit;

    public UpdateAssetHandler(IAssetRepository repo, IAuditLogger audit)
    {
        _repo = repo;
        _audit = audit;
    }

    public async Task<AssetDto?> Handle(UpdateAssetCommand cmd)
    {
        var asset = await _repo.GetByIdAsync(cmd.AssetId);
        if (asset is null) return null;

        asset.Update(cmd.Name, cmd.Type, cmd.Location, cmd.Description);
        await _repo.UpdateAsync(asset);
        await _audit.LogAsync("Asset.Updated", asset.AssetId);
        return AssetDto.From(asset);
    }
}
