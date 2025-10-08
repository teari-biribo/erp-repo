
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class WorkflowService : IWorkflowService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public WorkflowService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<WorkflowInstanceStep?> GetPendingStepAsync(int instanceId)
    {
        return await _context.WorkflowInstanceSteps
            .Include(s => s.WorkflowStep)
            .Where(s => s.WorkflowInstanceId == instanceId && s.Status == "Pending")
            .OrderBy(s => s.WorkflowStep.StepOrder)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ApproveStepAsync(int instanceId, string userId, string? comments = null)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        var pendingStep = await GetPendingStepAsync(instanceId);
        if (pendingStep == null)
            return false; // Nothing to approve

        // Validate authorization
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var inRole = await _userManager.IsInRoleAsync(user, pendingStep.WorkflowStep.RoleName);

        if (pendingStep.AssignedUserId != userId && !inRole)
            return false; // Unauthorized user

        // Approve current step
        pendingStep.Status = "Approved";
        pendingStep.Comments = comments;
        pendingStep.ActionDate = DateTime.UtcNow;

        // Check for next step
        var nextStep = await _context.WorkflowInstanceSteps
            .Include(s => s.WorkflowStep)
            .Where(s => s.WorkflowInstanceId == instanceId &&
                        s.WorkflowStep.StepOrder > pendingStep.WorkflowStep.StepOrder)
            .OrderBy(s => s.WorkflowStep.StepOrder)
            .FirstOrDefaultAsync();

        var instance = await _context.WorkflowInstances
            .FirstAsync(i => i.Id == instanceId);

        if (nextStep != null)
        {
            nextStep.Status = "Pending";
            instance.Status = "In Progress";

        }

        else
        {
            instance.Status = "Approved";
        }

        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return true;
    }

    public async Task<bool> RejectStepAsync(int instanceId, string userId, string? comments = null)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        var pendingStep = await GetPendingStepAsync(instanceId);
        if (pendingStep == null)
            return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var inRole = await _userManager.IsInRoleAsync(user, pendingStep.WorkflowStep.RoleName);
        if (pendingStep.AssignedUserId != userId && !inRole)
            return false; // Unauthorized user

        // Reject current step
        pendingStep.Status = "Rejected";
        pendingStep.Comments = comments;
        pendingStep.ActionDate = DateTime.UtcNow;

        // Mark entire workflow instance as Rejected
        var instance = await _context.WorkflowInstances
            .FirstAsync(i => i.Id == instanceId);
        instance.Status = "Rejected";

        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return true;
    }

    public async Task<Workflow> CreateWorkflowAsync(WorkflowCreateDto dto)
    {
        var workflow = new Workflow
        {
            Name = dto.Name,
            Description = dto.Description
        };

        foreach (var stepDto in dto.Steps.OrderBy(s => s.StepOrder))
        {
            var step = new WorkflowStep
            {
                StepOrder = stepDto.StepOrder,
                StepName = stepDto.StepName,
                RoleName = stepDto.RoleName,
                StepType = stepDto.StepType,
                IsFinalStep = stepDto.IsFinalStep,
                Workflow = workflow
            };
            workflow.Steps.Add(step);
        }

        _context.Workflows.Add(workflow);
        await _context.SaveChangesAsync();
        return workflow;
    }

    public async Task StartWorkflowAsync(string workflowName, string userId, string entityType, int entityId)
    {
        var workflow = await _context.Workflows
            .Include(w => w.Steps.OrderBy(s => s.StepOrder))
            .FirstOrDefaultAsync(w => w.Name == workflowName);

        if (workflow == null)
            throw new Exception("Workflow not found.");

        var instance = new WorkflowInstance
        {
            WorkflowId = workflow.Id,
            EntityType = entityType,
            EntityId = entityId,
            Status = "In Progress",
            CreatedAt = DateTime.UtcNow,
            Steps = workflow.Steps.Select(s => new WorkflowInstanceStep
            {
                WorkflowStepId = s.Id,
                Status = s.StepOrder == 1 ? "Active" : "Pending",
                AssignedUserId = s.RoleName == "Employee" ? userId : null
            }).ToList()
        };

        _context.WorkflowInstances.Add(instance);
        await _context.SaveChangesAsync();
    }

    public async Task CompleteStepAsync(Guid instanceStepId, bool approved, string comments)
    {
        var step = await _context.WorkflowInstanceSteps
            .Include(s => s.WorkflowInstance)
            .ThenInclude(i => i.Steps)
            .FirstOrDefaultAsync(s => s.Id.Equals(instanceStepId));

        if (step == null) throw new Exception("Step not found.");

        step.Status = approved ? "Approved" : "Rejected";
        step.Comments = comments;
        step.CompletedAt = DateTime.UtcNow;

        var instance = step.WorkflowInstance;

        if (!approved)
        {
            // Go back to previous step (e.g., employee re-submits)
            var previousStep = instance.Steps
                .OrderByDescending(s => s.WorkflowStep.StepOrder)
                .FirstOrDefault(s => s.WorkflowStep.StepOrder < step.WorkflowStep.StepOrder);

            if (previousStep != null)
            {
                previousStep.Status = "Active";
                instance.Status = "Revisions Required";
            }
        }
        else
        {
            // Move to next step
            var nextStep = instance.Steps
                .OrderBy(s => s.WorkflowStep.StepOrder)
                .FirstOrDefault(s => s.WorkflowStep.StepOrder > step.WorkflowStep.StepOrder);

            if (nextStep != null)
            {
                nextStep.Status = "Active";
            }
            else
            {
                // Completed
                instance.Status = "Completed";
            }
        }

        await _context.SaveChangesAsync();
    }

}