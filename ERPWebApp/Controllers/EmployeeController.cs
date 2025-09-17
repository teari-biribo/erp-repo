using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPWebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    // -------Old Org Chart Method--------
    // [HttpGet]
    // public async Task<ActionResult<OrgChartDto>> GetOrgChartData()
    // {
    //     // Fetch all employees and their current roles
    //     var employees = await _dbContext.Employees.Include(e => e.CurrentRole).ToListAsync();

    //     // Fetch all reporting relationships
    //     var roleReportings = await _dbContext.RoleReportings.ToListAsync();

    //     // Create the NodeDataArray
    //     var nodeDataArray = new List<NodeDataDto>();

    //     foreach (var employee in employees)
    //     {
    //         // Find the role this employee reports to
    //         var reportsToRoleId = roleReportings
    //             .FirstOrDefault(rr => rr.DirectReportId == employee.CurrentRoleId)?
    //             .ReportsToId;

    //         // Find the employee who holds the reporting role
    //         var parentEmployee = employees
    //             .FirstOrDefault(e => e.CurrentRoleId == reportsToRoleId);

    //         var nodeData = new NodeDataDto
    //         {
    //             Key = employee.EmployeeId,
    //             Name = $"{employee.FirstName} {employee.LastName}",
    //             Title = employee.CurrentRole.Title
    //         };

    //         // Only add the Parent property if a parent employee exists
    //         if (parentEmployee != null)
    //         {
    //             nodeData.Parent = parentEmployee.EmployeeId;
    //         }

    //         nodeDataArray.Add(nodeData);
    //     }

    //     return Ok(nodeDataArray);
    // }

    [HttpGet("orgchart")]
    [Authorize(Roles = "HR Admin,HR Manager,Employee")]
    public async Task<ActionResult<OrgChartDto>> GetOrgChartData()
    {
        var orgChart = await _service.GetOrgChartDataAsync();
        return Ok(orgChart);
    }

    [HttpGet]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        var employees = await _service.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _service.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto dto)
    {
        var created = await _service.AddAsync(dto);
        return CreatedAtAction(nameof(GetEmployee), new { id = created.EmployeeId }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto dto)
    {
        if (id != dto.EmployeeId) return BadRequest();

        var success = await _service.UpdateAsync(dto);
        if (!success) return NotFound();

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "HR Admin,HR Manager")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();

        return Ok();
    }
}
