
public class WorkflowInstance
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public Workflow Workflow { get; set; } = null!;
    public string EntityType { get; set; } = string.Empty; // e.g., "LeaveRequest"
    public int EntityId { get; set; } // Link to LeaveRequest.Id
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public ICollection<WorkflowInstanceStep> Steps { get; set; } = new List<WorkflowInstanceStep>();
    public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(new DateTime(2020, 1, 6), DateTimeKind.Utc);
}