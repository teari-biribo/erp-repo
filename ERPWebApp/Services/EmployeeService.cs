using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ApplicationDbContext _dbcontext;

    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository repository, IMapper mapper, ApplicationDbContext dbcontext)
    {
        _repository = repository;
        _dbcontext = dbcontext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> AddAsync(EmployeeDto dto)
    {
        var employee = _mapper.Map<Employee>(dto);
        var added = await _repository.AddAsync(employee, dto.ManagerRoleId);
        return _mapper.Map<EmployeeDto>(added);
    }

    public async Task<bool> UpdateAsync(EmployeeDto dto)
    {
        var existing = await _repository.GetByIdAsync(dto.EmployeeId);
        if (existing == null) return false;

        var employee = _mapper.Map<Employee>(dto);
        await _repository.UpdateAsync(employee, dto.CurrentRoleId, dto.ManagerRoleId);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id)) return false;
        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<OrgChartDto> GetOrgChartDataAsync()
    {
        var employees = (await _repository.GetAllWithRolesAsync()).ToList();
        var roleReportings = (await _repository.GetAllRoleReportingsAsync()).ToList();

        var nodeDataArray = new List<NodeDataDto>();

        foreach (var employee in employees)
        {
            // Find the role this employee reports to
            var reportsToRoleId = roleReportings
                .FirstOrDefault(rr => rr.DirectReportId == employee.CurrentRoleId)?
                .ReportsToId;

            // Find the employee who holds the reporting role
            var parentEmployee = employees
                .FirstOrDefault(e => e.CurrentRoleId == reportsToRoleId);

            var nodeData = new NodeDataDto
            {
                Key = employee.EmployeeId,
                Name = $"{employee.FirstName} {employee.LastName}",
                Title = employee.CurrentRole.Title
            };

            if (parentEmployee != null)
            {
                nodeData.Parent = parentEmployee.EmployeeId;
            }

            nodeDataArray.Add(nodeData);
        }

        return new OrgChartDto { NodeDataArray = nodeDataArray };
    }

    // Reuse old UpdateEmployeeRoleAsync but keep it inside service
    public async Task UpdateEmployeeRoleAsync(int employeeId, int newRoleId)
    {
        var employee = await _dbcontext.Employees
            .Include(e => e.CurrentRole)
            .Include(e => e.RoleHistory)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        using (var transaction = await _dbcontext.Database.BeginTransactionAsync())
        {
            try
            {
                var currentHistoryRecord = employee.RoleHistory
                    .FirstOrDefault(h => h.EndDate == null);

                if (currentHistoryRecord != null)
                {
                    currentHistoryRecord.EndDate = DateTime.UtcNow;
                }

                var newHistoryRecord = new EmployeeRoleHistory
                {
                    EmployeeId = employee.EmployeeId,
                    RoleId = newRoleId,
                    StartDate = DateTime.UtcNow,
                    EndDate = null
                };
                _dbcontext.EmployeeRoleHistory.Add(newHistoryRecord);

                employee.CurrentRoleId = newRoleId;

                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}