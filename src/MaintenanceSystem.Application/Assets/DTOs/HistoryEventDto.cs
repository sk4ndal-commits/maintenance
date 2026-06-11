namespace MaintenanceSystem.Application.Assets.DTOs;

public record HistoryEventDto(
    Guid Id,
    DateTime Timestamp,
    string EventType,
    string Title,
    string? Details,
    Guid? WorkOrderId,
    string? WorkOrderTitle,
    string? Actor
);
