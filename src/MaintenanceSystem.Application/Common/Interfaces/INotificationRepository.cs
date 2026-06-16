using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Notification notification);
    Task MarkAsReadAsync(Guid notificationId);
}
