using System;
using System.Threading.Tasks;
using MaintenanceSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public NotificationController(INotificationRepository repository, ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications()
    {
        var notifications = await _repository.GetByUserIdAsync(_currentUser.UserId);
        return Ok(notifications);
    }

    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _repository.MarkAsReadAsync(id);
        return NoContent();
    }
}
