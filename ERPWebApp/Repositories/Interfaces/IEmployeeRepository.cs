
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllWithRolesAsync();
    Task<IEnumerable<RoleReporting>> GetAllRoleReportingsAsync();
    Task<Employee> AddAsync(Employee employee, int? managerRoleId);
    Task UpdateAsync(Employee employee, int? newRoleId, int? managerRoleId);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}