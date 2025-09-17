
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _dbContext.Employees
            .Include(e => e.CurrentRole)
            .Include(e => e.RoleHistory)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _dbContext.Employees
            .Include(e => e.CurrentRole)
            .Include(e => e.RoleHistory)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);
    }

    public async Task<Employee> AddAsync(Employee employee, int? managerRoleId)
    {
        // Save employee
        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync();

        // Create RoleHistory entry
        var history = new EmployeeRoleHistory
        {
            EmployeeId = employee.EmployeeId,
            RoleId = employee.CurrentRoleId,
            StartDate = DateTime.UtcNow
        };
        _dbContext.EmployeeRoleHistory.Add(history);

        // Add RoleReporting relationship (if manager provided)
        if (managerRoleId.HasValue)
        {
            var reporting = new RoleReporting
            {
                DirectReportId = employee.CurrentRoleId,
                ReportsToId = managerRoleId.Value
            };
            _dbContext.Set<RoleReporting>().Add(reporting);
        }

        await _dbContext.SaveChangesAsync();
        return employee;
    }

    public async Task<IEnumerable<Employee>> GetAllWithRolesAsync()
    {
        return await _dbContext.Employees
            .Include(e => e.CurrentRole)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoleReporting>> GetAllRoleReportingsAsync()
    {
        return await _dbContext.RoleReportings.ToListAsync();
    }

    public async Task UpdateAsync(Employee employee, int? newRoleId, int? managerRoleId)
    {
        var existing = await _dbContext.Employees
            .Include(e => e.RoleHistory)
            .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

        if (existing == null) return;

        // Update basic info
        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;

        // If role changed → update history
        if (newRoleId.HasValue && existing.CurrentRoleId != newRoleId.Value)
        {
            // End old role
            var currentHistory = existing.RoleHistory?
                .FirstOrDefault(rh => rh.EndDate == null);
            if (currentHistory != null)
                currentHistory.EndDate = DateTime.UtcNow;

            // Start new role
            existing.CurrentRoleId = newRoleId.Value;
            var newHistory = new EmployeeRoleHistory
            {
                EmployeeId = existing.EmployeeId,
                RoleId = newRoleId.Value,
                StartDate = DateTime.UtcNow
            };
            _dbContext.EmployeeRoleHistory.Add(newHistory);
        }

        // Update manager relationship
        if (managerRoleId.HasValue)
        {
            var existingReporting = await _dbContext.Set<RoleReporting>()
                .FirstOrDefaultAsync(r => r.DirectReportId == existing.CurrentRoleId);

            if (existingReporting != null)
            {
                existingReporting.ReportsToId = managerRoleId.Value;
            }
            else
            {
                _dbContext.Set<RoleReporting>().Add(new RoleReporting
                {
                    DirectReportId = existing.CurrentRoleId,
                    ReportsToId = managerRoleId.Value
                });
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee != null)
        {
            // Remove role history
            var history = _dbContext.EmployeeRoleHistory
                .Where(h => h.EmployeeId == id);
            _dbContext.EmployeeRoleHistory.RemoveRange(history);

            // Remove reporting relationships
            var reports = _dbContext.Set<RoleReporting>()
                .Where(r => r.DirectReportId == employee.CurrentRoleId || r.ReportsToId == employee.CurrentRoleId);
            _dbContext.Set<RoleReporting>().RemoveRange(reports);

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Employees.AnyAsync(e => e.EmployeeId == id);
    }
}