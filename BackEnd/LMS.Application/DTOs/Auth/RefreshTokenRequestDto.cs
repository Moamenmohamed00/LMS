namespace LMS.Application.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

public sealed class RefreshTokenRequestDto
{
    [Required] public string RefreshToken { get; set; } = string.Empty;
}
