
using System.ComponentModel.DataAnnotations;

public class WorkflowStep
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public Workflow Workflow { get; set; } = null!;
    public int StepOrder { get; set; }

    [MaxLength(100)]
    public string StepName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string RoleName { get; set; } = string.Empty; // Role required for approval

    [MaxLength(50)]
    public string StepType { get; set; } // e.g. "UserTask", "Approval", "System"
    public bool IsFinalStep { get; set; } = false;
}