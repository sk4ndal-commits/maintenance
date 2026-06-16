using MaintenanceSystem.Application.Common.Interfaces;
using MaintenanceSystem.Domain.Entities;

namespace MaintenanceSystem.Application.Users.Commands;

public class InviteUserHandler
{
    private readonly ITechnicianRepository _repo;
    private readonly PasswordResetService _resetService;
    private readonly IEmailService _emailService;

    public InviteUserHandler(ITechnicianRepository repo, PasswordResetService resetService, IEmailService emailService)
    {
        _repo = repo;
        _resetService = resetService;
        _emailService = emailService;
    }

    public async Task HandleAsync(string name, string email, string role)
    {
        // 1. User erstellen (mit zufälligem, unbenutzbarem Passwort)
        var user = Technician.Create(name, email, Guid.NewGuid().ToString(), role);
        await _repo.AddAsync(user);

        // 2. Reset-Token generieren
        var token = await _resetService.GenerateResetTokenAsync(email);
        
        // 3. E-Mail versenden
        var link = $"https://app.url/reset-password?token={token}";
        await _emailService.SendInvitationEmailAsync(email, link);
    }
}
