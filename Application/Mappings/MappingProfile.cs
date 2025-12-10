using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.City;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        CityMappingProfile();
        CountryMappingProfile();
    }
}