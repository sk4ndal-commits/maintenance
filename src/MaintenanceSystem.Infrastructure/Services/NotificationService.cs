using System;
using System.Threading.Tasks;
using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task NotifyUserAsync(Guid userId, string message, string type = "Info")
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            Type = type
        };
        await _repository.AddAsync(notification);
    }
}
