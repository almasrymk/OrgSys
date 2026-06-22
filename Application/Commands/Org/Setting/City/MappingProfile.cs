using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.City.Commands;

public partial class MappingProfile : Profile
{
    public void CityMappingProfile()
    {
        #region City
        CreateMap<City, CityModelView>()
        .ForMember(dest => dest.CountryName,opt => opt.MapFrom(src => src.Country.Name));
        CreateMap<CityModelView, City>();

        CreateMap<City, CreateCityCommand>();
        CreateMap<CreateCityCommand, City>();
        CreateMap<City, UpdateCityCommand>();
        CreateMap<UpdateCityCommand, City>();        
        CreateMap<City, DeleteCityCommand>();
        CreateMap<DeleteCityCommand, City>();

        CreateMap<CityModelView, CreateCityCommand>();
        CreateMap<CreateCityCommand, CityModelView>();
        CreateMap<CityModelView, UpdateCityCommand>();
        CreateMap<UpdateCityCommand, CityModelView>();
        #endregion
    }
}