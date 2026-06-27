using System.ComponentModel.DataAnnotations;

namespace LMS.Application.Settings;

public sealed class AppUrlSettings
{
    public const string SectionName = "App";
    [Required, Url] public string ClientUrl { get; set; } = string.Empty;
}
