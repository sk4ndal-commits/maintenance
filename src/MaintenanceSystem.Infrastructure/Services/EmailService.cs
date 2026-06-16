using MaintenanceSystem.Application.Common.Interfaces;

namespace MaintenanceSystem.Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendInvitationEmailAsync(string email, string invitationLink)
    {
        // Simple console log as mock implementation
        Console.WriteLine($"[EMAIL] Sending invitation to {email}: {invitationLink}");
        return Task.CompletedTask;
    }
}
