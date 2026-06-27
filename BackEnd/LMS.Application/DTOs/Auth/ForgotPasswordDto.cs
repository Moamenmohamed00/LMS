namespace LMS.Application.DTOs.Auth;
using System.ComponentModel.DataAnnotations;
    public sealed class ForgotPasswordDto{
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    }
