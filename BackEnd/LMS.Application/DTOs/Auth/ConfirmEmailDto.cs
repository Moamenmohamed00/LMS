namespace LMS.Application.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

public sealed class ConfirmEmailDto
{
    public Guid UserId { get; set; }
    [Required] public string Token { get; set; } = string.Empty;
}
