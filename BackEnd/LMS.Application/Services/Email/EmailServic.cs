using System.Net;
using System.Net.Mail;
using LMS.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LMS.Application.Services.Email;

public sealed class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpSettings _smtp;
    private readonly AppUrlSettings _app;

    public EmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtp, IOptions<AppUrlSettings> app)
    {
        _logger = logger;
        _smtp = smtp.Value;
        _app = app.Value;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        if (!_smtp.Enabled)
        {
            _logger.LogWarning("SMTP is disabled. Email to {Email} with subject {Subject} was not sent", to, subject);
            return true;
        }

        try
        {
            using var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                EnableSsl = _smtp.EnableSsl,
                Credentials = new NetworkCredential(_smtp.Username, _smtp.Password)
            };
            using var message = new MailMessage
            {
                From = new MailAddress(_smtp.FromEmail, _smtp.AppName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);
            await client.SendMailAsync(message);
            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send email to {Email}", to);
            return false;
        }
    }

    public Task<bool> SendPasswordResetEmailAsync(string email, string token)
    {
        var url = $"{BaseUrl}/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
        return SendEmailAsync(email, $"{_smtp.AppName} - Reset password", LinkMessage("Reset your password", url));
    }

    public Task<bool> SendEmailConfirmationAsync(string email, Guid userId, string token)
    {
        var url = $"{BaseUrl}/confirm-email?userId={userId}&token={Uri.EscapeDataString(token)}";
        return SendEmailAsync(email, $"{_smtp.AppName} - Confirm email", LinkMessage("Confirm your email", url));
    }

    public Task<bool> SendWelcomeEmailAsync(string email, string firstName) =>
        SendEmailAsync(email, $"Welcome to {_smtp.AppName}", $"<h2>Welcome, {WebUtility.HtmlEncode(firstName)}</h2><p>Your account is ready.</p>");

    public Task<bool> SendAccountApprovedEmailAsync(string email, string firstName) =>
        SendEmailAsync(email, $"{_smtp.AppName} - Account approved", $"<h2>Hello, {WebUtility.HtmlEncode(firstName)}</h2><p>Your instructor account was approved.</p><p><a href='{BaseUrl}/login'>Log in</a></p>");

    private string BaseUrl => _app.ClientUrl.TrimEnd('/');
    private string LinkMessage(string title, string url) => $"<h2>{WebUtility.HtmlEncode(_smtp.AppName)}</h2><p><a href='{url}'>{title}</a></p>";
}
