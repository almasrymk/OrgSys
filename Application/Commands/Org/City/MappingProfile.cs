using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.City.Commands;

public partial class MappingProfile : Profile
{
    public void CityMappingProfile()
    {
        #region City
        CreateMap<City, CityModelView>()
        .ForMember(dest => dest.CountryName,opt => opt.MapFrom(src => src.Country.Name));
        CreateMap<CityModelView, City>();
        CreateMap<City, CreateCommand>();
        CreateMap<CreateCommand, City>();
        CreateMap<City, UpdateCommand>();
        CreateMap<UpdateCommand, City>();
        CreateMap<City, DeleteCommand>();
        CreateMap<DeleteCommand, City>();
        #endregion
    }
}