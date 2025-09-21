
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Success, string Message)> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return (false, "Invalid login attempt.");
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            loginDto.Password,
            isPersistent: loginDto.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: loginDto.RememberMe);
            return (true, "Login successful.");
        }

        return (false, "Invalid login attempt.");
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto registerDto)
    {
        var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
        if (userExists != null)
            return (false, "User already exists.");

        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Default role = Employee
        var role = string.IsNullOrEmpty(registerDto.Role) ? "Employee" : registerDto.Role;
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return (false, $"Role '{role}' does not exist.");
        }

        await _userManager.AddToRoleAsync(user, role);

        return (true, $"User '{registerDto.Email}' registered successfully with role '{role}'.");
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
