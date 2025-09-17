
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        if (token == null)
            return Unauthorized("Invalid login attempt.");

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var (success, message) = await _authService.RegisterAsync(registerDto);
        if (!success)
            return BadRequest(message);

        return Ok(new { message });
    }


}