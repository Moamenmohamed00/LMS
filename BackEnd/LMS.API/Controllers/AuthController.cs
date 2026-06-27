using LMS.Application.DTOs.Auth;
using LMS.Application.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public Task<IActionResult> Register(RegisterRequestDto request) => Respond(_auth.Register(request));
    [HttpPost("login")]
    public Task<IActionResult> Login(LoginRequestDto request) => Respond(_auth.Login(request));
    [HttpPost("refresh")]
    public Task<IActionResult> Refresh(RefreshTokenRequestDto request) => Respond(_auth.RefreshToken(request));
    [HttpPost("logout")]
    public Task<IActionResult> Logout(RefreshTokenRequestDto request) => Respond(_auth.RevokeToken(request));
    [HttpPost("forgot-password")]
    public Task<IActionResult> ForgotPassword(ForgotPasswordDto request) => Respond(_auth.ForgotPassword(request));
    [HttpPost("reset-password")]
    public Task<IActionResult> ResetPassword(ResetPasswordDto request) => Respond(_auth.ResetPassword(request));
    [HttpPost("confirm-email")]
    public Task<IActionResult> ConfirmEmail(ConfirmEmailDto request) => Respond(_auth.ConfirmEmail(request));
    [HttpPost("resend-confirmation")]
    public Task<IActionResult> ResendConfirmation(ForgotPasswordDto request) => Respond(_auth.ResendConfirmation(request));

    [Authorize(Roles = "Admin")]
    [HttpPost("instructors/{userId:guid}/approve")]
    public Task<IActionResult> ApproveInstructor(Guid userId) => Respond(_auth.ApproveInstructor(userId));

    private async Task<IActionResult> Respond(Task<AuthResponseDto> operation)
    {
        var result = await operation;
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
