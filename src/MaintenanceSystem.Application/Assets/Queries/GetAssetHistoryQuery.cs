namespace MaintenanceSystem.Application.Assets.Queries;

public record GetAssetHistoryQuery(
    Guid AssetId,
    string? EventType = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 50
);
