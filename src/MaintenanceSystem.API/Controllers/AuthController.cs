using MaintenanceSystem.Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly PasswordResetService _passwordResetService;

    public AuthController(PasswordResetService passwordResetService)
    {
        _passwordResetService = passwordResetService;
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
    {
        var token = await _passwordResetService.GenerateResetTokenAsync(req.Email);
        
        // TODO: In a real scenario, send email with token
        // For now, return token to client for simulation
        return Ok(new { token });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
    {
        var success = await _passwordResetService.ResetPasswordAsync(req.Token, req.NewPassword);
        if (!success) return BadRequest(new { message = "Invalid token or password" });
        return NoContent();
    }
}

public record ForgotPasswordRequest(string Email);
public record ResetPasswordRequest(string Token, string NewPassword);
