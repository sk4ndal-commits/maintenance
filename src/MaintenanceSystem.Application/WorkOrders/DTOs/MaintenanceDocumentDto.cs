using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.DTOs;

public record MaintenanceDocumentDto(
    Guid Id,
    Guid WorkOrderId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string? Notes,
    DateTime UploadedAt,
    string UploadedBy
)
{
    public static MaintenanceDocumentDto From(MaintenanceDocument d) => new(
        d.Id, d.WorkOrderId, d.FileName, d.ContentType,
        d.FileSizeBytes, d.Notes, d.UploadedAt, d.UploadedBy);
}
