
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("employee")]

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    public int CurrentRoleId { get; set; }
    public Role? CurrentRole { get; set; }

    // Navigation property for employement history
    public ICollection<EmployeeRoleHistory>? RoleHistory { get; set; }

    // Many-to-One: Employee -> Role
    public int RoleId { get; set; }
    public Role? Role { get; set; }
}