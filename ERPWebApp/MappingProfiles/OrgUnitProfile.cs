using AutoMapper;

public class OrgUnitProfile : Profile
{
    public OrgUnitProfile()
    {
        CreateMap<OrgUnit, OrgUnitDto>().ReverseMap();
    }
}