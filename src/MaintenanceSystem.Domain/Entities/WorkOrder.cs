namespace MaintenanceSystem.Domain.Entities;

public class WorkOrder
{
    public Guid WorkOrderId { get; private set; }
    public Guid AssetId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public WorkOrderStatus Status { get; private set; }
    public string Priority { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? AssignedTechnicianId { get; private set; }
    public string? AssignedTechnicianName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private WorkOrder() { }

    public void Assign(Guid technicianId, string technicianName)
    {
        AssignedTechnicianId = technicianId;
        AssignedTechnicianName = technicianName;
        Status = WorkOrderStatus.Assigned;
    }

    public static WorkOrder Create(Guid assetId, string title, string priority, string? description = null) => new()
    {
        WorkOrderId = Guid.NewGuid(),
        AssetId = assetId,
        Title = title,
        Status = WorkOrderStatus.Open,
        Priority = priority,
        Description = description,
        CreatedAt = DateTime.UtcNow
    };
}
