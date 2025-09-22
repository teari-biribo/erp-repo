

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int roleId, int orgUnitId);
    Task<Role> AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int roleId, int orgUnitId);
    Task<bool> ExistsAsync(int id);
}
