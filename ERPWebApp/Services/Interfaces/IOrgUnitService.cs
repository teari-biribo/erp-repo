
public interface IOrgUnitService
{
    Task<IEnumerable<OrgUnitDto>> GetAllAsync();
    Task<OrgUnitDto?> GetByIdAsync(int id);
    Task<OrgUnitDto> AddAsync(OrgUnitDto dto);
    Task<bool> UpdateAsync(int id, OrgUnitDto dto);
    Task<bool> DeleteAsync(int id);
}
