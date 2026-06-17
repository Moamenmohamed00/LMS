using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LMS.Application.Services;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infrastructure.Identity
{
    public class JwtTokenService:IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LMSDBContext _dbcontext;
        
        public JwtTokenService(IOptionsSnapshot<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager, LMSDBContext dbcontext)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _dbcontext = dbcontext;
        }

        public async Task<RefreshToken> GenerateRefreshToken(ApplicationUser user)
        {
            var token= new RefreshToken{
                UserId=user.Id,
                Token=await GenerateTokenAsync(user,new List<string>()),
                CreatedAt=DateTime.UtcNow,
                ExpiresAt=DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
            };

            await _dbcontext.RefreshTokens.AddAsync(token);
            await _dbcontext.SaveChangesAsync();
            return token;
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user,IEnumerable<string> roles)//2 ways
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? "")
            };
            // var userRoles = await _userManager.GetRolesAsync(user);
            // foreach(var role in userRoles)
            // {
            //     claims.Add(new Claim(ClaimTypes.Role, role));
            // }
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = credentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task RevokeRefreshToken(string refreshToken)
        {
            var token= await _dbcontext.RefreshTokens.FirstOrDefaultAsync(t=>t.Token==refreshToken);
            if(token==null) return;
            token.RevokedAt=DateTime.UtcNow;
            _dbcontext.RefreshTokens.Update(token);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RevokeUserRefreshTokens(Guid userId)
        {
            var tokens=await _dbcontext.RefreshTokens.Where(t=>t.UserId==userId).ToListAsync();
            foreach(var token in tokens)
            {
                token.RevokedAt=DateTime.UtcNow;
                _dbcontext.RefreshTokens.Update(token);
            }
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> ValidateRefreshToken(string refreshToken, ApplicationUser user)
        {
            var existToken= await _dbcontext.RefreshTokens.FirstOrDefaultAsync(t=>t.Token==refreshToken&&t.UserId==user.Id);
            if(existToken==null|| !existToken.IsActive)
             return null;
            existToken.RevokedAt=DateTime.UtcNow;
            _dbcontext.RefreshTokens.Update(existToken);
            var newToken=await GenerateRefreshToken(user);
            await _dbcontext.SaveChangesAsync();
            return newToken;
        }
    }
}