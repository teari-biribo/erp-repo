
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> AddAsync(EmployeeDto dto);
    Task<bool> UpdateAsync(EmployeeDto dto);
    Task<OrgChartDto> GetOrgChartDataAsync();
    Task UpdateEmployeeRoleAsync(int employeeId, int newRoleId);
    Task<bool> DeleteAsync(int id);
}