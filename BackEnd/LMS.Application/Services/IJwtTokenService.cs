using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}