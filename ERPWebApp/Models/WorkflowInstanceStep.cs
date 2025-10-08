
public class WorkflowInstanceStep
{
    public int Id { get; set; }
    public int WorkflowInstanceId { get; set; }
    public WorkflowInstance WorkflowInstance { get; set; } = null!;
    public int WorkflowStepId { get; set; }
    public WorkflowStep WorkflowStep { get; set; } = null!;
    public string AssignedUserId { get; set; } = string.Empty; // ASP.NET Identity User
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public DateTime? ActionDate { get; set; } = DateTime.SpecifyKind(new DateTime(2020, 1, 6), DateTimeKind.Utc);
    public DateTime? CompletedAt { get; set; } = DateTime.SpecifyKind(new DateTime(2020, 1, 6), DateTimeKind.Utc);
    public string? Comments { get; set; }
}