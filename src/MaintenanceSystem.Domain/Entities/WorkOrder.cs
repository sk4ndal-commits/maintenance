namespace MaintenanceSystem.Domain.Entities;

public class WorkOrder
{
    public Guid WorkOrderId { get; private set; }
    public Guid AssetId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public WorkOrderStatus Status { get; private set; }
    public WorkOrderPriority Priority { get; private set; }
    public string? Description { get; private set; }
    public DateTime? DueDate { get; private set; }
    public Guid? AssignedTechnicianId { get; private set; }
    public string? AssignedTechnicianName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? CompletionNotes { get; private set; }

    private WorkOrder() { }

    public void Transition(WorkOrderStatus newStatus, IEnumerable<ChecklistStep>? steps = null)
    {
        var valid = (Status, newStatus) switch
        {
            (WorkOrderStatus.Assigned,   WorkOrderStatus.InProgress) => true,
            (WorkOrderStatus.InProgress, WorkOrderStatus.Done)       => true,
            _ => false
        };

        if (!valid)
            throw new InvalidOperationException(
                $"Transition from {Status} to {newStatus} is not allowed.");

        if (newStatus == WorkOrderStatus.Done)
        {
            var mandatorySteps = steps ?? [];
            var incomplete = mandatorySteps.Where(s => s.IsMandatory && !s.IsCompleted).ToList();
            if (incomplete.Count != 0)
                throw new InvalidOperationException(
                    $"Cannot complete: {incomplete.Count} mandatory checklist step(s) not done.");

            CompletedAt = DateTime.UtcNow;
        }

        Status = newStatus;
    }

    public void Complete(IEnumerable<ChecklistStep> steps, string? completionNotes)
    {
        var mandatorySteps = steps.ToList();
        var incomplete = mandatorySteps.Where(s => s.IsMandatory && !s.IsCompleted).ToList();
        if (incomplete.Count != 0)
            throw new InvalidOperationException(
                $"Cannot complete: {incomplete.Count} mandatory checklist step(s) not done.");

        Status = WorkOrderStatus.Done;
        CompletedAt = DateTime.UtcNow;
        CompletionNotes = completionNotes;
    }

    public void Assign(Guid technicianId, string technicianName)
    {
        AssignedTechnicianId = technicianId;
        AssignedTechnicianName = technicianName;
        Status = WorkOrderStatus.Assigned;
    }

    public static WorkOrder Create(Guid assetId, string title, WorkOrderPriority priority, string? description = null, DateTime? dueDate = null) => new()
    {
        WorkOrderId = Guid.NewGuid(),
        AssetId = assetId,
        Title = title,
        Status = WorkOrderStatus.Open,
        Priority = priority,
        Description = description,
        DueDate = dueDate,
        CreatedAt = DateTime.UtcNow
    };
}
