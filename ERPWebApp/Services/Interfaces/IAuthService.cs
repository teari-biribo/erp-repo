

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto loginDto);
    Task<(bool Success, string Message)> RegisterAsync(RegisterDto registerDto);
}