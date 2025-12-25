using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        CityMappingProfile();
        CountryMappingProfile();
    }
}