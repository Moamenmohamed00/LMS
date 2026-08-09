using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LMS.Application.Services.Iinfra;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infrastructure.Identity;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    private readonly LMSDBContext _db;

    public JwtTokenService(IOptionsSnapshot<JwtSettings> settings, LMSDBContext db)
    {
        _settings = settings.Value;
        _db = db;
    }

    public Task<string> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.UserName ?? string.Empty)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };
        var handler = new JwtSecurityTokenHandler();
        return Task.FromResult(handler.WriteToken(handler.CreateToken(descriptor)));
    }

    public async Task<string> GenerateRefreshToken(ApplicationUser user)
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = Hash(rawToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiryInDays)
        });
        await _db.SaveChangesAsync();
        return rawToken;
    }

    public async Task RevokeRefreshToken(string refreshToken)
    {
        var hash = Hash(refreshToken);
        var token = await _db.RefreshTokens.SingleOrDefaultAsync(item => item.Token == hash);
        if (token is null || token.RevokedAt is not null) return;
        token.RevokedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task RevokeUserRefreshTokens(Guid userId)
    {
        await _db.RefreshTokens.Where(item => item.UserId == userId && item.RevokedAt == null)
            .ExecuteUpdateAsync(update => update.SetProperty(item => item.RevokedAt, DateTime.UtcNow));
    }

    public async Task<(ApplicationUser User, string RefreshToken)?> RotateRefreshToken(string refreshToken)
    {
        var hash = Hash(refreshToken);
        var current = await _db.RefreshTokens.Include(item => item.User)
            .SingleOrDefaultAsync(item => item.Token == hash);
        if (current is null || !current.IsActive) return null;

        current.RevokedAt = DateTime.UtcNow;
        var replacement = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = current.UserId,
            Token = Hash(replacement),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiryInDays)
        });
        await _db.SaveChangesAsync();
        return (current.User, replacement);
    }

    private static string Hash(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
