namespace Catalog.Application;

﻿using AutoMapper;
using Catalog.Application.Attributes.Commands;

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