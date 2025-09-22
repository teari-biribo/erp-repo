

public interface IAuthService
{
    Task<(bool Success, string Message)> LoginAsync(LoginDto loginDto);
    Task<(bool Success, string Message)> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();
}