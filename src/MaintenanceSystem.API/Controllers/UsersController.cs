using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, [FromServices] CreateUserHandler handler)
    {
        var createReq = new CreateUserRequest(req.Name, req.Email, req.Password, "Technician");
        var user = await handler.HandleAsync(createReq);
        return CreatedAtAction(nameof(Login), new { id = user.TechnicianId },
            new { user.TechnicianId, user.Name, user.Email, user.Role });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, [FromServices] LoginHandler handler)
    {
        var token = await handler.HandleAsync(req.Email, req.Password);
        if (token is null)
            return Unauthorized(new { message = "Invalid credentials or inactive account" });
        return Ok(new { token });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromServices] ITechnicianRepository repo)
    {
        var users = await repo.GetAllAsync();
        return Ok(users.Select(u => new { u.TechnicianId, u.Name, u.Email, u.Role, u.IsActive }));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest req, [FromServices] CreateUserHandler handler)
    {
        var user = await handler.HandleAsync(req);
        return CreatedAtAction(nameof(GetAll), new { id = user.TechnicianId },
            new { user.TechnicianId, user.Name, user.Email, user.Role });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest req, [FromServices] UpdateUserHandler handler)
    {
        await handler.HandleAsync(id, req);
        return NoContent();
    }

    [HttpPatch("{id:guid}/active")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetActive(Guid id, [FromBody] SetActiveRequest req, [FromServices] SetUserActiveHandler handler)
    {
        await handler.HandleAsync(id, req.IsActive);
        return NoContent();
    }

    [HttpPatch("{id:guid}/password")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] AdminResetPasswordRequest req, [FromServices] ResetPasswordHandler handler)
    {
        await handler.HandleAsync(id, req.NewPassword);
        return NoContent();
    }

    [HttpPost("invite")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Invite([FromBody] InviteRequest req, [FromServices] InviteUserHandler handler)
    {
        await handler.HandleAsync(req.Name, req.Email, req.Role);
        return NoContent();
    }
}

public record RegisterRequest(string Name, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record SetActiveRequest(bool IsActive);
public record AdminResetPasswordRequest(string NewPassword);
public record InviteRequest(string Name, string Email, string Role);
