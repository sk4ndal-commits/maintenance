namespace MaintenanceSystem.Domain.Entities;

public class MaintenanceDocument
{
    public Guid Id { get; private set; }
    public Guid WorkOrderId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public string StoragePath { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public string? Notes { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string UploadedBy { get; private set; } = string.Empty;

    private MaintenanceDocument() { }

    public static MaintenanceDocument Create(
        Guid workOrderId, string fileName, string contentType,
        string storagePath, long fileSizeBytes, string? notes, string uploadedBy) => new()
    {
        Id = Guid.NewGuid(),
        WorkOrderId = workOrderId,
        FileName = fileName,
        ContentType = contentType,
        StoragePath = storagePath,
        FileSizeBytes = fileSizeBytes,
        Notes = notes,
        UploadedAt = DateTime.UtcNow,
        UploadedBy = uploadedBy
    };
}
