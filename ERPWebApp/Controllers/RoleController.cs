
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HR Admin, HR Manager")]

public class RoleController : ControllerBase
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        var roles = await _service.GetAllAsync();
        return Ok(roles);
    }

    [HttpGet("{roleId}")]
    public async Task<ActionResult<RoleDto>> GetRole(int roleId, [FromQuery] int orgUnitId)
    {
        var role = await _service.GetByIdAsync(roleId, orgUnitId);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] RoleDto dto)
    {
        var created = await _service.AddAsync(dto);
        return CreatedAtAction(nameof(GetRole), new { id = created.RoleId }, created);
    }

    [HttpPut("{roleId}/orgunit/{orgUnitId}")]
    public async Task<IActionResult> UpdateRole(int roleId, int orgUnitId, [FromBody] RoleDto dto)
    {
        if (roleId != dto.RoleId || orgUnitId != dto.OrgUnitId)
        {
            return BadRequest("Mismatched RoleId or OrgUnitId");
        }

        var existing = await _service.GetByIdAsync(roleId, orgUnitId);
        if (existing == null)
        {
            return NotFound();
        }

        var updated = await _service.UpdateAsync(dto);
        if (!updated) return StatusCode(500, "Failed to update role");
        return Ok();
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(int roleId, [FromQuery] int orgUnitId)
    {
        var success = await _service.DeleteAsync(roleId, orgUnitId);
        if (!success) return NotFound();
        return Ok();
    }
}