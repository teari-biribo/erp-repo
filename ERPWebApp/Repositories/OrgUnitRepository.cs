
using Microsoft.EntityFrameworkCore;

public class OrgUnitRepository : IOrgUnitRepository
{
    private readonly ApplicationDbContext _dbContext;
    public OrgUnitRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrgUnit?> GetByIdAsync(int id)
    {
        return await _dbContext.OrganizationalUnits.FindAsync(id);
    }

    public async Task<OrgUnit> AddAsync(OrgUnit orgUnit)
    {
        _dbContext.OrganizationalUnits.Add(orgUnit);
        await _dbContext.SaveChangesAsync();
        return orgUnit;
    }

    public async Task UpdateAsync(OrgUnit orgUnit)
    {
        _dbContext.Entry(orgUnit).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.OrganizationalUnits.FindAsync(id);
        if (entity != null)
        {
            _dbContext.OrganizationalUnits.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.OrganizationalUnits.AnyAsync(e => e.UnitId == id);
    }

    public async Task<IEnumerable<OrgUnit>> GetAllAsync()
    {
        return await _dbContext.OrganizationalUnits.ToListAsync();
    }
}