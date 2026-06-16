using System;
using System.Threading.Tasks;

namespace MaintenanceSystem.Application.Common.Interfaces;

public interface INotificationService
{
    Task NotifyUserAsync(Guid userId, string message, string type = "Info");
}
