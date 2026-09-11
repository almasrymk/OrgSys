namespace MasterData.Application;

﻿using AutoMapper;
using MasterData.Application.Countries.Commands;

public partial class MappingProfile : Profile
{
    public void CountryMappingProfile()
    {
        #region Country
        CreateMap<Country, CountryDto>();
        CreateMap<CountryDto, Country>();
        CreateMap<Country, CreateCountryCommand>();
        CreateMap<CreateCountryCommand, Country>();
        CreateMap<Country, UpdateCountryCommand>();
        CreateMap<UpdateCountryCommand, Country>();
        CreateMap<Country, DeleteCountryCommand>();
        CreateMap<DeleteCountryCommand, Country>();
        #endregion
    }
}
