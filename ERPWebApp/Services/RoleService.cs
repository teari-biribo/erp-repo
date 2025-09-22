
using AutoMapper;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetByIdAsync(int roleId, int orgUnitId)
    {
        var role = await _repository.GetByIdAsync(roleId, orgUnitId);
        return role == null ? null : _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> AddAsync(RoleDto dto)
    {
        var entity = _mapper.Map<Role>(dto);
        var added = await _repository.AddAsync(entity);
        return _mapper.Map<RoleDto>(added);
    }

    public async Task<bool> UpdateAsync(RoleDto dto)
    {
        var existing = await _repository.GetByIdAsync(dto.RoleId, dto.OrgUnitId);
        if (existing == null) return false;

        _mapper.Map(dto, existing); // update entity with new values
        await _repository.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int roleId, int orgUnitId)
    {
        var existing = await _repository.GetByIdAsync(roleId, orgUnitId);
        if (existing == null) return false;

        await _repository.DeleteAsync(roleId, orgUnitId);
        return true;
    }
}