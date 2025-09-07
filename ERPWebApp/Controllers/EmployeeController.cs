using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERPWebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    public EmployeeController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // [HttpGet]
    // public async Task<List<Employee>> Get()
    // {
    //     return await _dbContext.Employees.ToListAsync();
    // }

    [HttpGet]
    public async Task<ActionResult<OrgChartDto>> GetOrgChartData()
    {
        // Fetch all employees and their current roles
        var employees = await _dbContext.Employees
            .Include(e => e.CurrentRole)
            .ToListAsync();

        // Fetch all reporting relationships
        var reporting = await _dbContext.RoleReportings.ToListAsync();

        // Create the NodeDataArray
        var nodeDataArray = employees.Select(e => new NodeDataDto
        {
            Key = e.EmployeeId,
            Name = $"{e.FirstName} {e.LastName}",
            Title = e.CurrentRole.Title
        }).ToList();

        // Create the LinkDataArray
        var linkDataArray = reporting.Select(r => new LinkDataDto
        {
            From = employees.First(e => e.CurrentRoleId == r.ReportsToId).EmployeeId,
            To = employees.First(e => e.CurrentRoleId == r.DirectReportId).EmployeeId
        }).ToList();

        // Handle multiple roots: You'll need to decide how to handle multiple trees.
        // For a single chart, you would filter to a single root.
        // For multiple charts, you could create a different endpoint.
        // The below approach assumes a single connected component.

        var chartData = new OrgChartDto
        {
            NodeDataArray = nodeDataArray,
            LinkDataArray = linkDataArray
        };

        return Ok(chartData);
    }

    [HttpGet("{id}")]
    public async Task<Employee?> GetById(int id)
    {
        return await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
    }

    //create employee
    //HttpPost]
    // public async Task<ActionResult> Create([FromBody] Employee employee)
    // {
    //     if (string.IsNullOrWhiteSpace(employee.FirstName) ||
    //         string.IsNullOrWhiteSpace(employee.Email) ||
    //         string.IsNullOrWhiteSpace(employee.Password)
    //     )

    //     {
    //         return BadRequest("Invalid Request");
    //     }

    //     //implement code to hash password

    //     await _dbContext.Employees.AddAsync(employee);
    //     await _dbContext.SaveChangesAsync();

    //     return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    // }

    //create role
    // [HttpPost]
    // public async Task<ActionResult> CreateRole([FromBody] Employee employee)
    // {
    //     if (string.IsNullOrWhiteSpace(employee.Name) ||
    //         string.IsNullOrWhiteSpace(employee.Email) ||
    //         string.IsNullOrWhiteSpace(employee.Password)
    //     )

    //     {
    //         return BadRequest("Invalid Request");
    //     }

    //     await _dbContext.Employees.AddAsync(employee);
    //     await _dbContext.SaveChangesAsync();

    //     return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);

    //     // _context.Employees.Add(employee);
    //     // await _context.SaveChangesAsync();

    //     // // Return the newly created employee with its new ID
    //     // return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
    // }

    // [HttpPut]
    // public async Task<ActionResult> Update([FromBody] Employee employee)
    // {
    //     if (
    //         string.IsNullOrWhiteSpace(employee.Name) ||
    //         string.IsNullOrWhiteSpace(employee.Email) ||
    //         string.IsNullOrWhiteSpace(employee.Password)
    //     )

    //     {
    //         return BadRequest("Invalid Request");
    //     }

    //     //implement code to hash new password

    //     _dbContext.Employees.Update(employee);
    //     await _dbContext.SaveChangesAsync();

    //     return Ok();
    // }

    // [HttpDelete("{id}")]
    // public async Task<ActionResult> Delete(int id)
    // {
    //     var employee = await GetById(id);

    //     if (employee is null)
    //         return NotFound();

    //     _dbContext.Employees.Remove(employee);
    //     await _dbContext.SaveChangesAsync();

    //     return Ok();
    // }
}
