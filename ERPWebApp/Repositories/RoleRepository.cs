
using Microsoft.EntityFrameworkCore;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;
    public RoleRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Role?> GetByIdAsync(int roleId, int orgUnitId)
    {
        // return await _dbContext.Roles.FindAsync(id);
        return await _dbContext.Roles
        .FirstOrDefaultAsync(r => r.RoleId == roleId && r.OrgUnitId == orgUnitId);
    }

    public async Task<Role> AddAsync(Role role)
    {
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();
        return role;
    }

    public async Task UpdateAsync(Role role)
    {
        _dbContext.Entry(role).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int roleId, int orgUnitId)
    {
        var role = await GetByIdAsync(roleId, orgUnitId);
        if (role != null)
        {
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Roles.AnyAsync(e => e.RoleId == id);
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _dbContext.Roles.ToListAsync();
    }
}