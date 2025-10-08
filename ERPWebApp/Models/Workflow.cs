
public class Workflow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
}