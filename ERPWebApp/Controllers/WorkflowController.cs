

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class WorkflowController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWorkflowService _workflowService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public WorkflowController(
        ApplicationDbContext context,
        IWorkflowService workflowService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _workflowService = workflowService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // ------------------------------------------------------------
    // APPROVER ACTIONS
    // ------------------------------------------------------------

    // GET: Approvals
    [HttpGet]
    public async Task<IActionResult> MyApprovals()
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);

        var pendingApprovals = await _context.WorkflowInstanceSteps
            .Include(s => s.WorkflowInstance)
            .ThenInclude(i => i.Workflow)
            .Include(s => s.WorkflowStep)
            .Where(s =>
                (s.AssignedUserId == user.Id || roles.Contains(s.WorkflowStep.RoleName)) &&
                s.Status == "Pending")
            .ToListAsync();

        return View(pendingApprovals);
    }

    // GET: Workflow/Details/{id} 
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var instance = await _context.WorkflowInstances
            .Include(i => i.Workflow)
            .Include(i => i.Steps)
                .ThenInclude(s => s.WorkflowStep)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instance == null)
            return NotFound();

        return View(instance);
    }

    // POST: Workflow/Approve
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int instanceId, string? comments)
    {
        var userId = _userManager.GetUserId(User);
        var result = await _workflowService.ApproveStepAsync(instanceId, userId, comments);

        if (!result)
        {
            TempData["Error"] = "You are not authorized or this step is not pending.";
            return RedirectToAction("MyApprovals");
        }

        TempData["Success"] = "Approval recorded successfully.";
        return RedirectToAction("MyApprovals");
    }

    // POST: Workflow/Reject
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int instanceId, string? comments)
    {
        var userId = _userManager.GetUserId(User);
        var result = await _workflowService.RejectStepAsync(instanceId, userId, comments);

        if (!result)
        {
            TempData["Error"] = "You are not authorized or this step is not pending.";
            return RedirectToAction("MyApprovals");
        }

        TempData["Success"] = "Rejection recorded successfully.";
        return RedirectToAction("MyApprovals");
    }

    // ------------------------------------------------------------
    // ADMIN MANAGEMENT
    // ------------------------------------------------------------

    [Authorize(Roles = "HR Admin")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var workflows = await _context.Workflows
            .Include(w => w.Steps.OrderBy(s => s.StepOrder))
            .ToListAsync();
        return View(workflows);
    }

    [Authorize(Roles = "HR Admin")]
    [HttpGet]
    // GET: /Workflow/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Workflow/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WorkflowCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            // Debug output to help trace what’s wrong
            var errors = string.Join("; ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            TempData["Error"] = $"Invalid input: {errors}";
            return View(dto);
        }

        await _workflowService.CreateWorkflowAsync(dto);

        TempData["Success"] = "Workflow created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "HR Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var workflow = await _context.Workflows
            .Include(w => w.Steps.OrderBy(s => s.StepOrder))
            .FirstOrDefaultAsync(w => w.Id == id);

        if (workflow == null)
            return NotFound();

        ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return View(workflow);
    }

    [Authorize(Roles = "HR Admin")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Workflow model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(model);
        }

        _context.Workflows.Update(model);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Workflow updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "HR Admin")]
    [HttpGet]
    public async Task<IActionResult> AddStep(int workflowId)
    {
        ViewBag.WorkflowId = workflowId;
        ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = users;
        return View(new WorkflowStep());
    }

    [Authorize(Roles = "HR Admin")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStep(int workflowId, WorkflowStep step, string? assignedUserId)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.WorkflowId = workflowId;
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(step);
        }

        step.WorkflowId = workflowId;
        _context.WorkflowSteps.Add(step);
        await _context.SaveChangesAsync();

        // Optionally assign a user directly to this step in existing workflows
        if (!string.IsNullOrEmpty(assignedUserId))
        {
            var instanceSteps = await _context.WorkflowInstanceSteps
                .Where(s => s.WorkflowStepId == step.Id)
                .ToListAsync();
            foreach (var s in instanceSteps)
                s.AssignedUserId = assignedUserId;

            await _context.SaveChangesAsync();
        }

        TempData["Success"] = "Step added successfully.";
        return RedirectToAction(nameof(Edit), new { id = workflowId });
    }

    [Authorize(Roles = "HR Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteStep(int id)
    {
        var step = await _context.WorkflowSteps.FindAsync(id);
        if (step == null)
            return NotFound();

        _context.WorkflowSteps.Remove(step);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Step deleted successfully.";
        return RedirectToAction(nameof(Edit), new { id = step.WorkflowId });
    }
}