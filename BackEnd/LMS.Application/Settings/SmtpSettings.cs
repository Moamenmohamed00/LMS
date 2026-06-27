using System.ComponentModel.DataAnnotations;

namespace LMS.Application.Settings;

public sealed class SmtpSettings
{
    public const string SectionName = "Smtp";
    public bool Enabled { get; set; }
    [Required] public string Host { get; set; } = string.Empty;
    [Range(1, 65535)] public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Required, EmailAddress] public string FromEmail { get; set; } = string.Empty;
    [Required] public string AppName { get; set; } = "LMS Academy";
}
