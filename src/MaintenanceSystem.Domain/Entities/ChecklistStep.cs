namespace MaintenanceSystem.Domain.Entities;

public class ChecklistStep
{
    public Guid Id { get; private set; }
    public Guid WorkOrderId { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public bool IsMandatory { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private ChecklistStep() { }

    public static ChecklistStep Create(Guid workOrderId, string label, bool isMandatory) => new()
    {
        Id = Guid.NewGuid(),
        WorkOrderId = workOrderId,
        Label = label,
        IsMandatory = isMandatory,
        IsCompleted = false
    };

    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void Uncomplete()
    {
        IsCompleted = false;
        CompletedAt = null;
    }
}
