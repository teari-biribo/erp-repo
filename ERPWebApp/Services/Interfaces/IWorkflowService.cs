
public interface IWorkflowService
{
    Task<bool> ApproveStepAsync(int instanceId, string userId, string? comments = null);
    Task<bool> RejectStepAsync(int instanceId, string userId, string? comments = null);
    Task<WorkflowInstanceStep?> GetPendingStepAsync(int instanceId);
    Task<Workflow> CreateWorkflowAsync(WorkflowCreateDto dto);
}