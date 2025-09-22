
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // Optional link to Employee (to tie authentication to HR data)
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}