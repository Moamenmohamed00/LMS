namespace LMS.Application.Services.Email;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
    Task<bool> SendPasswordResetEmailAsync(string email, string token);
    Task<bool> SendEmailConfirmationAsync(string email, Guid userId, string token);
    Task<bool> SendWelcomeEmailAsync(string email, string firstName);
    Task<bool> SendAccountApprovedEmailAsync(string email, string firstName);
}
