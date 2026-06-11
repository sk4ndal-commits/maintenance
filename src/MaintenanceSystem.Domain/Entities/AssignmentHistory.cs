namespace MaintenanceSystem.Domain.Entities;

public class AssignmentHistory
{
    public Guid Id { get; private set; }
    public Guid WorkOrderId { get; private set; }
    public Guid? TechnicianId { get; private set; }
    public string? TechnicianName { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public string AssignedBy { get; private set; } = string.Empty;

    private AssignmentHistory() { }

    public static AssignmentHistory Create(Guid workOrderId, Guid? technicianId, string? technicianName, string assignedBy) => new()
    {
        Id = Guid.NewGuid(),
        WorkOrderId = workOrderId,
        TechnicianId = technicianId,
        TechnicianName = technicianName,
        AssignedAt = DateTime.UtcNow,
        AssignedBy = assignedBy
    };
}
