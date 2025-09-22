
public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public int CurrentRoleId { get; set; }

    // Manager (reports-to relationship)
    public int? ManagerRoleId { get; set; }
}