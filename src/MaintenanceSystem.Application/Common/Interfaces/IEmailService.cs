namespace MaintenanceSystem.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendInvitationEmailAsync(string email, string invitationLink);
}
