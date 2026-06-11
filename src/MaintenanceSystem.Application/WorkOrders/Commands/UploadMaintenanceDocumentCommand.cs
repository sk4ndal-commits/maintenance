namespace MaintenanceSystem.Application.WorkOrders.Commands;

public record UploadMaintenanceDocumentCommand(
    Guid WorkOrderId,
    string FileName,
    string ContentType,
    Stream Content,
    long FileSizeBytes,
    string? Notes,
    string UploadedBy
);
