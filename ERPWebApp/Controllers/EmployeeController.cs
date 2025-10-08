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

    [HttpGet("orgchart")]
    [Authorize]
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
