using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Application.Users.Commands;
using MaintenanceSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly PasswordResetService _passwordResetService;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _tokenRepo;
    private readonly ITechnicianRepository _technicianRepo;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(
        PasswordResetService passwordResetService,
        IJwtService jwtService,
        IRefreshTokenRepository tokenRepo,
        ITechnicianRepository technicianRepo,
        IPasswordHasher passwordHasher)
    {
        _passwordResetService = passwordResetService;
        _jwtService = jwtService;
        _tokenRepo = tokenRepo;
        _technicianRepo = technicianRepo;
        _passwordHasher = passwordHasher;
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

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
    {
        var storedToken = await _tokenRepo.GetByTokenAsync(_passwordHasher.Hash(req.RefreshToken));
        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized();
        }

        var technician = await _technicianRepo.GetByIdAsync(storedToken.UserId);
        if (technician == null) return Unauthorized();

        var newAccessToken = _jwtService.GenerateToken(technician);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        
        storedToken.IsRevoked = true;
        await _tokenRepo.UpdateAsync(storedToken);

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = technician.TechnicianId,
            Token = _passwordHasher.Hash(newRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await _tokenRepo.AddAsync(newRefreshTokenEntity);

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> Revoke([FromBody] RevokeRequest req)
    {
        var storedToken = await _tokenRepo.GetByTokenAsync(_passwordHasher.Hash(req.RefreshToken));
        if (storedToken == null) return BadRequest();

        storedToken.IsRevoked = true;
        await _tokenRepo.UpdateAsync(storedToken);
        return NoContent();
    }
}

public record ForgotPasswordRequest(string Email);
public record ResetPasswordRequest(string Token, string NewPassword);
public record RefreshRequest(string RefreshToken);
public record RevokeRequest(string RefreshToken);
