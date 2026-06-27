using LMS.Domain.Entities;

namespace LMS.Domain.Entities
{
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public int Id { get; set; } 
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; } 
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => RevokedAt == null && !IsExpired; 

    public Guid UserId { get; set; } 
    public ApplicationUser User { get; set; } = null!;
}
}
