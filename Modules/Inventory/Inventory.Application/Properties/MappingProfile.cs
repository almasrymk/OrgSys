namespace Inventory.Application;

﻿using AutoMapper;
using Inventory.Application.Properties.Commands;

public partial class MappingProfile : Profile
{
    public void PropertyMappingProfile()
    {
        #region Property
        CreateMap<Property, PropertyDto>();
        CreateMap<PropertyDto, Property>();       
        #endregion
    }
}