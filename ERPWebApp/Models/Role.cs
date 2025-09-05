using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("role")]

public class Role
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    public string? Title { get; set; }

    // One-to-Many: Role -> OrganizationalUnit
    public int OrgUnitId { get; set; }
    public OrgUnit? OrgUnit { get; set; }


    // One-to-Many: Role -> Employee (via implicit join table)
    public ICollection<Employee>? Employees { get; set; }


    // Many-to-Many: Role -> Role (for reporting)
    public ICollection<RoleReporting>? DirectReports { get; set; }
    public ICollection<RoleReporting>? ReportsTo { get; set; }
}