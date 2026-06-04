namespace LMS.Application.Services;
public interface IEmailService
{
 Task<bool> SendEmailAsync(string to, string subject, string body);
 Task<bool> SendTemplateEmailAsync(string to, string subject, string templateName);
}