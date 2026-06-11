namespace MaintenanceSystem.Domain.Entities;

public class WorkOrder
{
    public Guid WorkOrderId { get; private set; }
    public Guid AssetId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public WorkOrderStatus Status { get; private set; }
    public string Priority { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private WorkOrder() { }

    public static WorkOrder Create(Guid assetId, string title, string priority) => new()
    {
        WorkOrderId = Guid.NewGuid(),
        AssetId = assetId,
        Title = title,
        Status = WorkOrderStatus.Open,
        Priority = priority,
        CreatedAt = DateTime.UtcNow
    };
}
