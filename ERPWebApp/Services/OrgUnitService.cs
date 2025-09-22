
using AutoMapper;

public class OrgUnitService : IOrgUnitService
{
    private readonly IOrgUnitRepository _repository;
    private readonly IMapper _mapper;

    public OrgUnitService(IOrgUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrgUnitDto>> GetAllAsync()
    {
        var units = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrgUnitDto>>(units);
    }

    public async Task<OrgUnitDto?> GetByIdAsync(int id)
    {
        var unit = await _repository.GetByIdAsync(id);
        return unit == null ? null : _mapper.Map<OrgUnitDto>(unit);
    }

    public async Task<OrgUnitDto> AddAsync(OrgUnitDto dto)
    {
        var entity = _mapper.Map<OrgUnit>(dto);
        var added = await _repository.AddAsync(entity);
        return _mapper.Map<OrgUnitDto>(added);
    }

    public async Task<bool> UpdateAsync(int id, OrgUnitDto dto)
    {
        if (id != dto.UnitId) return false;

        if (!await _repository.ExistsAsync(id))
            return false;

        var entity = _mapper.Map<OrgUnit>(dto);
        await _repository.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }
}