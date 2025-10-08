using System.ComponentModel.DataAnnotations;

public class WorkflowCreateDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(350)]
    public string? Description { get; set; } = string.Empty;

    public List<WorkflowStepDto> Steps { get; set; } = new();
}

public class WorkflowStepDto
{
    public int StepOrder { get; set; }

    [Required, MaxLength(100)]
    public string StepName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string RoleName { get; set; } = "System";
    public string StepType { get; set; } = "UserTask";
    public bool IsFinalStep { get; set; } = false;
}