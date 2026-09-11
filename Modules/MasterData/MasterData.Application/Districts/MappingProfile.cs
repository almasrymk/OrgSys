namespace MasterData.Application;

﻿using MasterData.Application.Districts.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void DistrictMappingProfile()
    {
        #region District
        CreateMap<District, DistrictDto>()
           .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
           .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name));
        CreateMap<DistrictDto, District>();

        CreateMap<District, CreateDistrictCommand>();
        CreateMap<CreateDistrictCommand, District>();
        CreateMap<District, UpdateDistrictCommand>();
        CreateMap<UpdateDistrictCommand, District>();
        CreateMap<District, DeleteDistrictCommand>();
        CreateMap<DeleteDistrictCommand, District>();

        CreateMap<DistrictDto, CreateDistrictCommand>();
        CreateMap<CreateDistrictCommand, DistrictDto>();
        CreateMap<DistrictDto, UpdateDistrictCommand>();
        CreateMap<UpdateDistrictCommand, DistrictDto>();
        #endregion
    }
}
