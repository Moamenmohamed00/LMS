using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user,IEnumerable<string> roles);
        Task<RefreshToken> GenerateRefreshToken(ApplicationUser user);
        Task RevokeRefreshToken(string refreshToken);
        Task RevokeUserRefreshTokens(Guid userId);
        Task<RefreshToken?> ValidateRefreshToken(string refreshToken,ApplicationUser user);
    }
}