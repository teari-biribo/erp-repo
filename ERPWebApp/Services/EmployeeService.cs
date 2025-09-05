using Microsoft.EntityFrameworkCore;

public class EmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateEmployeeRoleAsync(int employeeId, int newRoleId)
    {
        // Get the employee and their current role history record
        var employee = await _context.Employees
            .Include(e => e.CurrentRole)
            .Include(e => e.RoleHistory)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        // Use a transaction to ensure both updates succeed or fail together
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                // Find the currently active role history record for this employee
                var currentHistoryRecord = employee.RoleHistory
                    .FirstOrDefault(h => h.EndDate == null);

                if (currentHistoryRecord != null)
                {
                    // Update the end date for the old role
                    currentHistoryRecord.EndDate = DateTime.UtcNow;
                }

                // Create a new history record for the new role
                var newHistoryRecord = new EmployeeRoleHistory
                {
                    EmployeeId = employee.EmployeeId,
                    RoleId = newRoleId,
                    StartDate = DateTime.UtcNow,
                    EndDate = null // This is the new current record
                };
                _context.EmployeeRoleHistory.Add(newHistoryRecord);

                // Update the employee's current role
                employee.CurrentRoleId = newRoleId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}