using System.Net;
using LMS.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.AppName, _smtp.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            // MailKit's 'Auto' is usually best, but we will respect the EnableSsl flag for exact parity
            var secureSocketOptions = _smtp.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            await client.ConnectAsync(_smtp.Host, _smtp.Port, secureSocketOptions);

            if (!string.IsNullOrEmpty(_smtp.Username) && !string.IsNullOrEmpty(_smtp.Password))
            {
                await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
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
