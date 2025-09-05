using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrgUnit
{
    [Key]
    public int UnitId { get; set; }

    [Required]
    public required string Name { get; set; }

    // One-to-Many: OrganizationalUnit -> Roles
    public ICollection<Role>? Roles { get; set; }

    // One-to-One: OrganizationalUnit -> GM
    public int? GMId { get; set; }

    [ForeignKey("GMId")]
    public Employee? GeneralManager { get; set; }

    // One-to-One: OrganizationalUnit -> LM
    public int? LMId { get; set; }

    [ForeignKey("LMId")]
    public Employee? LineManager { get; set; }

    // One-to-One: OrgUnit -> HRBP
    public int? HRBPId { get; set; }

    [ForeignKey("HRBPId")]
    public Employee? HRBusinessPartner { get; set; }
}
