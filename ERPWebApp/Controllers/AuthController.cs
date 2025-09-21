
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("Auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("Login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(); // Renders Login.cshtml
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }

        var (success, message) = await _authService.LoginAsync(loginDto);
        if (success)
        {
            // Redirect to requested URL or default Home
            return Redirect(returnUrl ?? Url.Action("Index", "Home")!);
        }

        ModelState.AddModelError(string.Empty, message);
        return View(loginDto);
    }

    [HttpGet("Register")]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("Register")]
    [Authorize(Roles = "HR Admin,HR Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }

        var (success, message) = await _authService.RegisterAsync(registerDto);
        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Login", "Auth");
        }

        ModelState.AddModelError(string.Empty, message);
        return View(registerDto);
    }

    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied() => View();
}