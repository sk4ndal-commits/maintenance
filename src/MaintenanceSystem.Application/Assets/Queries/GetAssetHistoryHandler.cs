using MaintenanceSystem.Application.Assets.DTOs;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Assets.Queries;

public class GetAssetHistoryHandler
{
    private readonly IWorkOrderRepository _woRepo;
    private readonly IAssignmentHistoryRepository _assignRepo;
    private readonly IChecklistStepRepository _stepsRepo;
    private readonly IMaintenanceDocumentRepository _docRepo;

    public GetAssetHistoryHandler(
        IWorkOrderRepository woRepo,
        IAssignmentHistoryRepository assignRepo,
        IChecklistStepRepository stepsRepo,
        IMaintenanceDocumentRepository docRepo)
    {
        _woRepo = woRepo;
        _assignRepo = assignRepo;
        _stepsRepo = stepsRepo;
        _docRepo = docRepo;
    }

    public async Task<(IEnumerable<HistoryEventDto> Events, int Total)> Handle(GetAssetHistoryQuery q)
    {
        var workOrders = (await _woRepo.GetByAssetIdAsync(q.AssetId, int.MaxValue)).ToList();
        var woIds = workOrders.Select(w => w.WorkOrderId).ToHashSet();

        var events = new List<HistoryEventDto>();

        foreach (var wo in workOrders)
        {
            events.Add(new HistoryEventDto(
                Guid.NewGuid(), wo.CreatedAt, "WorkOrderCreated",
                $"Work Order erstellt: {wo.Title}",
                $"Priorität: {wo.Priority}", wo.WorkOrderId, wo.Title, "system"));
        }

        foreach (var wo in workOrders.Where(w => w.Status == WorkOrderStatus.Done && w.CompletedAt != null))
        {
            events.Add(new HistoryEventDto(
                Guid.NewGuid(), wo.CompletedAt!.Value, "WorkOrderCompleted",
                $"Work Order abgeschlossen: {wo.Title}",
                wo.CompletionNotes, wo.WorkOrderId, wo.Title,
                wo.AssignedTechnicianName));
        }

        foreach (var woId in woIds)
        {
            var assignments = await _assignRepo.GetByWorkOrderIdAsync(woId);
            var wo = workOrders.First(w => w.WorkOrderId == woId);
            foreach (var a in assignments)
            {
                events.Add(new HistoryEventDto(
                    a.Id, a.AssignedAt, "WorkOrderAssigned",
                    $"Zugewiesen an: {a.TechnicianName ?? "–"}",
                    null, woId, wo.Title, a.AssignedBy));
            }
        }

        foreach (var woId in woIds)
        {
            var steps = await _stepsRepo.GetByWorkOrderIdAsync(woId);
            var wo = workOrders.First(w => w.WorkOrderId == woId);
            foreach (var s in steps.Where(s => s.IsCompleted && s.CompletedAt != null))
            {
                events.Add(new HistoryEventDto(
                    s.Id, s.CompletedAt!.Value, "ChecklistStepCompleted",
                    $"Checklistenpunkt: {s.Label}",
                    s.IsMandatory ? "Pflichtschritt" : null,
                    woId, wo.Title, null));
            }
        }

        foreach (var woId in woIds)
        {
            var docs = await _docRepo.GetByWorkOrderIdAsync(woId);
            var wo = workOrders.First(w => w.WorkOrderId == woId);
            foreach (var d in docs)
            {
                events.Add(new HistoryEventDto(
                    d.Id, d.UploadedAt, "DocumentUploaded",
                    $"Dokument hochgeladen: {d.FileName}",
                    d.Notes, woId, wo.Title, d.UploadedBy));
            }
        }

        var filtered = events.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(q.EventType))
            filtered = filtered.Where(e => e.EventType == q.EventType);
        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var s = q.Search.ToLowerInvariant();
            filtered = filtered.Where(e =>
                e.Title.ToLowerInvariant().Contains(s) ||
                (e.Details?.ToLowerInvariant().Contains(s) ?? false) ||
                (e.WorkOrderTitle?.ToLowerInvariant().Contains(s) ?? false));
        }

        var sorted = filtered.OrderByDescending(e => e.Timestamp).ToList();
        var total = sorted.Count;
        var paged = sorted.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize);

        return (paged, total);
    }
}
