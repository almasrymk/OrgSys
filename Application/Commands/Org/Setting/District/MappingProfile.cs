using Application.Commands.Org.Setting.District.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void DistrictMappingProfile()
    {
        #region District
        CreateMap<District, DistrictModelView>()
           .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
           .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name));
        CreateMap<DistrictModelView, District>();

        CreateMap<District, CreateDistrictCommand>();
        CreateMap<CreateDistrictCommand, District>();
        CreateMap<District, UpdateDistrictCommand>();
        CreateMap<UpdateDistrictCommand, District>();
        CreateMap<District, DeleteDistrictCommand>();
        CreateMap<DeleteDistrictCommand, District>();

        CreateMap<DistrictModelView, CreateDistrictCommand>();
        CreateMap<CreateDistrictCommand, DistrictModelView>();
        CreateMap<DistrictModelView, UpdateDistrictCommand>();
        CreateMap<UpdateDistrictCommand, DistrictModelView>();
        #endregion
    }
}