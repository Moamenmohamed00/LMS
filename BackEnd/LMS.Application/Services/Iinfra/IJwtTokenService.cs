using LMS.Domain.Entities;
namespace LMS.Application.Services.Iinfra
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user,IEnumerable<string> roles);
        Task<string> GenerateRefreshToken(ApplicationUser user);
        Task RevokeRefreshToken(string refreshToken);
        Task RevokeUserRefreshTokens(Guid userId);
        Task<(ApplicationUser User, string RefreshToken)?> RotateRefreshToken(string refreshToken);
    }
}
