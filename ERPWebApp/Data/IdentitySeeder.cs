
using Microsoft.AspNetCore.Identity;


public class IdentitySeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentitySeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedUsersAndRolesAsync()
    {
        // Create hasher
        var hasher = new PasswordHasher<ApplicationUser>();

        // Define the user roles
        string[] roles = { "HR Admin", "HR Manager", "Employee" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // HR Admin User
        var hrAdminUser = new ApplicationUser
        {
            UserName = "hradmin",
            NormalizedUserName = "HRADMIN",
            Email = "hradmin@testcarpentersfiji.com",
            NormalizedEmail = "HRADMIN@TESTCARPENTERSFIJI.COM",
            FirstName = "HR",
            LastName = "Admin",
            EmailConfirmed = true
        };
        hrAdminUser.PasswordHash = hasher.HashPassword(hrAdminUser, "Admin@123");

        var result = await _userManager.CreateAsync(hrAdminUser);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(hrAdminUser, "HR Admin");
        }

        // HR Manager User
        var hrManagerUser = new ApplicationUser
        {
            UserName = "hrmanager",
            NormalizedUserName = "HRMANAGER",
            Email = "hrmanager@testcarpentersfiji.com",
            NormalizedEmail = "HRMANAGER@TESTCARPENTERSFIJI.COM",
            FirstName = "HR",
            LastName = "Manager",
            EmailConfirmed = true
        };
        hrManagerUser.PasswordHash = hasher.HashPassword(hrManagerUser, "Manager@123");

        result = await _userManager.CreateAsync(hrManagerUser);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(hrManagerUser, "HR Manager");
        }

        // Regular Employee User
        var employeeUser = new ApplicationUser
        {
            UserName = "employee",
            NormalizedUserName = "EMPLOYEE",
            Email = "employee@testcarpentersfiji.com",
            NormalizedEmail = "EMPLOYEE@TESTCARPENTERSFIJI.COM",
            FirstName = "Regular",
            LastName = "Employee",
            EmailConfirmed = true
        };
        employeeUser.PasswordHash = hasher.HashPassword(employeeUser, "Employee@123");

        result = await _userManager.CreateAsync(employeeUser);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(employeeUser, "Employee");
        }
    }
}