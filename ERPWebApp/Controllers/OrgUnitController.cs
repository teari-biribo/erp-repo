using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HR Admin, HR Manager")]
public class OrgUnitController : ControllerBase
{
    private readonly IOrgUnitService _service;
    // private readonly UserManager<IdentityUser> _userManager;
    public OrgUnitController(IOrgUnitService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrgUnitDto>>> GetOrgUnits()
    {
        var units = await _service.GetAllAsync();
        return Ok(units);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrgUnitDto>> GetOrgUnit(int id)
    {
        var unit = await _service.GetByIdAsync(id);
        if (unit == null) return NotFound();
        return Ok(unit);
    }

    [HttpPost]
    public async Task<ActionResult<OrgUnitDto>> CreateOrgUnit([FromBody] OrgUnitDto dto)
    {
        var created = await _service.AddAsync(dto);
        return CreatedAtAction(nameof(GetOrgUnit), new { id = created.UnitId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrgUnit(int id, [FromBody] OrgUnitDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrgUnit(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return Ok();
    }
}