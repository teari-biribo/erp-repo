
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class WorkflowInstanceFactory
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public WorkflowInstanceFactory(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<WorkflowInstance> CreateInstanceAsync(
        int workflowId, string entityType, int entityId)
    {
        var workflow = await _context.Workflows
            .Include(w => w.Steps.OrderBy(s => s.StepOrder))
            .FirstAsync(w => w.Id == workflowId);

        var instance = new WorkflowInstance
        {
            WorkflowId = workflow.Id,
            EntityType = entityType,
            EntityId = entityId,
            Status = "Pending"
        };

        // Create instance steps and assign approvers
        foreach (var step in workflow.Steps)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(step.RoleName);
            var approver = usersInRole.FirstOrDefault(); // Pick first user (or handle multiple)

            instance.Steps.Add(new WorkflowInstanceStep
            {
                WorkflowStepId = step.Id,
                AssignedUserId = approver?.Id ?? string.Empty,
                Status = step.StepOrder == 1 ? "Pending" : "Waiting"
            });
        }

        _context.WorkflowInstances.Add(instance);
        await _context.SaveChangesAsync();
        return instance;
    }
}