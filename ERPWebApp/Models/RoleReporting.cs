using System.ComponentModel.DataAnnotations.Schema;

[Table("role_reporting")]

public class RoleReporting
{
    [ForeignKey("DirectReport")]
    public int DirectReportId { get; set; }
    public Role? DirectReport { get; set; }

    [ForeignKey("ReportsTo")]
    public int ReportsToId { get; set; }
    public Role? ReportsTo { get; set; }
}
