namespace LMS.Application.IRepositories;
using LMS.Application.DTOs.Auth;

public interface IAuthRepository
{
    Task<AuthResponseDto> Login(LoginRequestDto request);
    Task<AuthResponseDto> Register(RegisterRequestDto request);
    Task<AuthResponseDto> ForgotPassword(ForgotPasswordDto request);
    Task<AuthResponseDto> ResetPassword(ResetPasswordDto request);
    Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request);
    Task<AuthResponseDto> RevokeToken(RefreshTokenRequestDto request);
    Task<AuthResponseDto> ConfirmEmail(ConfirmEmailDto request);
    Task<AuthResponseDto> ResendConfirmation(ForgotPasswordDto request);
    Task<AuthResponseDto> ApproveInstructor(Guid userId);
}
