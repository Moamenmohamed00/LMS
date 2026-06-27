namespace LMS.Application.Services.Auth;
using LMS.Application.DTOs.Auth;
using LMS.Application.IRepositories;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<AuthResponseDto> Login(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return AuthResponseDto.Failure("Invalid email or password.");

        if (user.Status == UserStatus.Suspended)
            return AuthResponseDto.Failure("Your account has been suspended.");

        if (user.Status == UserStatus.Pending)
            return AuthResponseDto.Failure("Your account is pending approval.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
            return AuthResponseDto.Failure("Account locked out. Try again later.");

        if (!result.Succeeded)
            return AuthResponseDto.Failure("Invalid email or password.");
        return await _authRepository.Login(request);
    }

    public async Task<AuthResponseDto> Register(RegisterRequestDto request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            // Instructors need admin approval; students are active immediately
            Status = request.Role.Equals("Instructor", StringComparison.OrdinalIgnoreCase)
                ? UserStatus.Pending
                : UserStatus.Active
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
            return AuthResponseDto.Failure(createResult.Errors.Select(e => e.Description).ToArray());

        // Ensure role exists, then assign
        var normalizedRole = request.Role.Equals("Instructor", StringComparison.OrdinalIgnoreCase)
            ? "Instructor" : "Student";

        if (!await _roleManager.RoleExistsAsync(normalizedRole))
            await _roleManager.CreateAsync(new IdentityRole<Guid>(normalizedRole));

        await _userManager.AddToRoleAsync(user, normalizedRole);

        // If instructor, don't generate token — they need approval first
        if (user.Status == UserStatus.Pending)
        {
            return new AuthResponseDto
            {
                IsSuccess = true,
                Errors = new List<string> { "Registration successful. Your account is pending admin approval." }
            };
        }
        // Validate role
        var validRoles = new[] { "Student", "Instructor" };
        if (!validRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
            return AuthResponseDto.Failure("Invalid role. Must be 'Student' or 'Instructor'.");

        return await _authRepository.Register(request);
    }

    public async Task<AuthResponseDto> ForgotPassword(ForgotPasswordDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that the user doesn't exist — security best practice
            return new AuthResponseDto { IsSuccess = true };
        }

        return await _authRepository.ForgotPassword(request);
    }

    public async Task<AuthResponseDto> ResetPassword(ResetPasswordDto request)
    {
         var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return AuthResponseDto.Failure("Invalid request.");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
            return AuthResponseDto.Failure(result.Errors.Select(e => e.Description).ToArray());
        return await _authRepository.ResetPassword(request);
    }

    public Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request) => _authRepository.RefreshToken(request);
    public Task<AuthResponseDto> RevokeToken(RefreshTokenRequestDto request) => _authRepository.RevokeToken(request);
    public Task<AuthResponseDto> ConfirmEmail(ConfirmEmailDto request) => _authRepository.ConfirmEmail(request);
    public Task<AuthResponseDto> ResendConfirmation(ForgotPasswordDto request) => _authRepository.ResendConfirmation(request);
    public Task<AuthResponseDto> ApproveInstructor(Guid userId) => _authRepository.ApproveInstructor(userId);
}
