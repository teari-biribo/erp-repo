using AutoMapper;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<OrgUnit, OrgUnitDto>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}