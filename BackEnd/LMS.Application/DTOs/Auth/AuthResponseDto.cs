namespace LMS.Application.DTOs.Auth;

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public UserDto? User { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static AuthResponseDto Success(string token, string refreshToken, UserDto user)
        => new() { IsSuccess = true, Token = token, RefreshToken = refreshToken, User = user };

    public static AuthResponseDto Failure(params string[] errors)
        => new() { IsSuccess = false, Errors = errors.ToList() };
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Role { get; set; } = string.Empty;
}
