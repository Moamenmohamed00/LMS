using System.ComponentModel.DataAnnotations;

namespace LMS.Infrastructure.Identity
{
    public class JwtSettings
    {
        [Required]
        public string Key { get; set; } = string.Empty;
        [Required]
        public string Issuer { get; set; } = string.Empty;
        [Required]
        public string Audience { get; set; } = string.Empty;
        [Required,Range(1,int.MaxValue,ErrorMessage="Expiry in minutes must be greater than 0")]
        public int ExpiryInMinutes { get; set; }
        [Required,Range(1,int.MaxValue,ErrorMessage="Refresh Token Expiry in days must be greater than 0")]
        public int RefreshTokenExpiryInDays { get; set; }
    }
}