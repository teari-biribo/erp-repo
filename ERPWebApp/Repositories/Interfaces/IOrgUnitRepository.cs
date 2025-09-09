

public interface IOrgUnitRepository
{
    Task<IEnumerable<OrgUnit>> GetAllAsync();
    Task<OrgUnit?> GetByIdAsync(int id);
    Task<OrgUnit> AddAsync(OrgUnit orgUnit);
    Task UpdateAsync(OrgUnit orgUnit);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
