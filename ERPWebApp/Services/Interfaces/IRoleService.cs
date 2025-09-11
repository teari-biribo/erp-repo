
public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(int roleId, int orgUnitId);
    Task<RoleDto> AddAsync(RoleDto dto);
    Task<bool> UpdateAsync(RoleDto dto);
    Task<bool> DeleteAsync(int roleId, int orgUnitId);
}
