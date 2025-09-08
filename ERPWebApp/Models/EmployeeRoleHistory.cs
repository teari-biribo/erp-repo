using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("employee_role_history")]

public class EmployeeRoleHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeRoleHistoryId { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public DateTime StartDate { get; set; } = DateTime.SpecifyKind(new DateTime(2020, 1, 6), DateTimeKind.Utc);
    public DateTime? EndDate { get; set; } // Nullable, as the current role has no end date yet
}
