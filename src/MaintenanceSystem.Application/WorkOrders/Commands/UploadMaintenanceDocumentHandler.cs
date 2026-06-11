using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.WorkOrders.DTOs;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.WorkOrders.Commands;

public class UploadMaintenanceDocumentHandler
{
    private readonly IFileStorageService _storage;
    private readonly IMaintenanceDocumentRepository _repo;
    private readonly IWorkOrderRepository _woRepo;
    private readonly IAuditLogger _audit;

    public UploadMaintenanceDocumentHandler(
        IFileStorageService storage,
        IMaintenanceDocumentRepository repo,
        IWorkOrderRepository woRepo,
        IAuditLogger audit)
    {
        _storage = storage;
        _repo = repo;
        _woRepo = woRepo;
        _audit = audit;
    }

    public async Task<MaintenanceDocumentDto> Handle(UploadMaintenanceDocumentCommand cmd)
    {
        var wo = await _woRepo.GetByIdAsync(cmd.WorkOrderId)
            ?? throw new KeyNotFoundException("WorkOrder not found");

        var storagePath = await _storage.StoreAsync(cmd.FileName, cmd.ContentType, cmd.Content);

        var doc = MaintenanceDocument.Create(
            wo.WorkOrderId, cmd.FileName, cmd.ContentType,
            storagePath, cmd.FileSizeBytes, cmd.Notes, cmd.UploadedBy);

        await _repo.AddAsync(doc);
        await _audit.LogAsync("MaintenanceDocument.Uploaded", wo.WorkOrderId, cmd.FileName);

        return MaintenanceDocumentDto.From(doc);
    }
}
