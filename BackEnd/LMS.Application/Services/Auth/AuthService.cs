namespace LMS.Application.Services.Auth;

using LMS.Application.DTOs.Auth;
using LMS.Application.Services;
using LMS.Application.Services.Email;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private static readonly string[] PublicRoles = ["Student", "Instructor"];
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly RoleManager<IdentityRole<Guid>> _roles;
    private readonly IJwtTokenService _tokens;
    private readonly IEmailService _email;

    public AuthService(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn,
        RoleManager<IdentityRole<Guid>> roles, IJwtTokenService tokens, IEmailService email)
    {
        _users = users;
        _signIn = signIn;
        _roles = roles;
        _tokens = tokens;
        _email = email;
    }

    public async Task<AuthResponseDto> Login(LoginRequestDto request)
    {
        var user = await _users.FindByEmailAsync(request.Email.Trim());
        if (user is null) return AuthResponseDto.Failure("Invalid email or password.");
        if (!user.EmailConfirmed) return AuthResponseDto.Failure("Confirm your email before signing in.");
        if (user.Status == UserStatus.Suspended) return AuthResponseDto.Failure("Your account is suspended.");
        if (user.Status == UserStatus.Pending) return AuthResponseDto.Failure("Your account is pending approval.");

        var result = await _signIn.CheckPasswordSignInAsync(user, request.Password, true);
        if (result.IsLockedOut) return AuthResponseDto.Failure("Account locked. Try again later.");
        if (!result.Succeeded) return AuthResponseDto.Failure("Invalid email or password.");
        return await CreateSession(user);
    }

    public async Task<AuthResponseDto> Register(RegisterRequestDto request)
    {
        var role = PublicRoles.FirstOrDefault(item => item.Equals(request.Role, StringComparison.OrdinalIgnoreCase));
        if (role is null) return AuthResponseDto.Failure("Role must be Student or Instructor.");
        var email = request.Email.Trim();
        if (await _users.FindByEmailAsync(email) is not null) return AuthResponseDto.Failure("Email is already registered.");

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Status = role == "Instructor" ? UserStatus.Pending : UserStatus.Active
        };
        var created = await _users.CreateAsync(user, request.Password);
        if (!created.Succeeded) return Failure(created);

        var assigned = await _users.AddToRoleAsync(user, role);
        if (!assigned.Succeeded)
        {
            await _users.DeleteAsync(user);
            return Failure(assigned);
        }

        var confirmationToken = await _users.GenerateEmailConfirmationTokenAsync(user);
        if (!await _email.SendEmailConfirmationAsync(email, user.Id, confirmationToken))
            return AuthResponseDto.Failure("Account created, but the confirmation email could not be sent. Use resend confirmation.");

        return new AuthResponseDto { IsSuccess = true, Message = "Registration successful. Check your email to confirm the account." };
    }

    public async Task<AuthResponseDto> ForgotPassword(ForgotPasswordDto request)
    {
        var user = await _users.FindByEmailAsync(request.Email.Trim());
        if (user is not null && user.EmailConfirmed)
        {
            var token = await _users.GeneratePasswordResetTokenAsync(user);
            await _email.SendPasswordResetEmailAsync(user.Email!, token);
        }
        return Accepted("If the account exists, a password-reset email has been sent.");
    }

    public async Task<AuthResponseDto> ResetPassword(ResetPasswordDto request)
    {
        var user = await _users.FindByEmailAsync(request.Email.Trim());
        if (user is null) return AuthResponseDto.Failure("Invalid password-reset request.");
        var result = await _users.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded) return Failure(result);
        await _tokens.RevokeUserRefreshTokens(user.Id);
        return Accepted("Password reset successfully.");
    }

    public async Task<AuthResponseDto> RefreshToken(RefreshTokenRequestDto request)
    {
        var rotated = await _tokens.RotateRefreshToken(request.RefreshToken);
        if (rotated is null || rotated.Value.User.Status != UserStatus.Active || !rotated.Value.User.EmailConfirmed)
            return AuthResponseDto.Failure("Invalid or expired refresh token.");
        var roles = await _users.GetRolesAsync(rotated.Value.User);
        var accessToken = await _tokens.GenerateTokenAsync(rotated.Value.User, roles);
        return AuthResponseDto.Success(accessToken, rotated.Value.RefreshToken, MapUser(rotated.Value.User, roles));
    }

    public async Task<AuthResponseDto> RevokeToken(RefreshTokenRequestDto request)
    {
        await _tokens.RevokeRefreshToken(request.RefreshToken);
        return Accepted("Signed out successfully.");
    }

    public async Task<AuthResponseDto> ConfirmEmail(ConfirmEmailDto request)
    {
        var user = await _users.FindByIdAsync(request.UserId.ToString());
        if (user is null) return AuthResponseDto.Failure("Invalid email-confirmation request.");
        if (user.EmailConfirmed) return Accepted("Email is already confirmed.");
        var result = await _users.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded) return Failure(result);
        if (user.Status == UserStatus.Active) await _email.SendWelcomeEmailAsync(user.Email!, user.FirstName);
        return Accepted(user.Status == UserStatus.Pending
            ? "Email confirmed. Your instructor account is pending approval."
            : "Email confirmed. You can now sign in.");
    }

    public async Task<AuthResponseDto> ResendConfirmation(ForgotPasswordDto request)
    {
        var user = await _users.FindByEmailAsync(request.Email.Trim());
        if (user is not null && !user.EmailConfirmed)
        {
            var token = await _users.GenerateEmailConfirmationTokenAsync(user);
            await _email.SendEmailConfirmationAsync(user.Email!, user.Id, token);
        }
        return Accepted("If confirmation is required, a new email has been sent.");
    }

    public async Task<AuthResponseDto> ApproveInstructor(Guid userId)
    {
        var user = await _users.FindByIdAsync(userId.ToString());
        if (user is null || !await _users.IsInRoleAsync(user, "Instructor"))
            return AuthResponseDto.Failure("Instructor was not found.");
        if (!user.EmailConfirmed) return AuthResponseDto.Failure("The instructor must confirm their email first.");
        user.Status = UserStatus.Active;
        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded) return Failure(result);
        await _email.SendAccountApprovedEmailAsync(user.Email!, user.FirstName);
        return Accepted("Instructor approved.");
    }

    private async Task<AuthResponseDto> CreateSession(ApplicationUser user)
    {
        var roles = await _users.GetRolesAsync(user);
        return AuthResponseDto.Success(await _tokens.GenerateTokenAsync(user, roles),
            await _tokens.GenerateRefreshToken(user), MapUser(user, roles));
    }

    private static UserDto MapUser(ApplicationUser user, IList<string> roles) => new()
    {
        Id = user.Id, Email = user.Email!, FirstName = user.FirstName, LastName = user.LastName,
        Role = roles.FirstOrDefault() ?? string.Empty
    };
    private static AuthResponseDto Failure(IdentityResult result) =>
        AuthResponseDto.Failure(result.Errors.Select(error => error.Description).ToArray());
    private static AuthResponseDto Accepted(string message) => new() { IsSuccess = true, Message = message };
}
