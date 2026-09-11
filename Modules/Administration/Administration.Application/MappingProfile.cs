namespace Administration.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        RoleMappingProfile();
        UserMappingProfile();
    }
}
