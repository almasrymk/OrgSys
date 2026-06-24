using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.Property.Commands;

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