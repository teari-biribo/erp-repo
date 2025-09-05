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

    [HttpGet]
    public async Task<List<Employee>> Get()
    {
        return await _dbContext.Employees.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<Employee?> GetById(int id)
    {
        return await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
    }

    //create employee
    [HttpPost]
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await GetById(id);

        if (employee is null)
            return NotFound();

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
