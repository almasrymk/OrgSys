using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void ProductUnitMappingProfile()
    {
        #region ProductUnit
        CreateMap<ProductUnit, ProductUnitDto>()
        .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));        
        CreateMap<ProductUnitDto, ProductUnit>()
        .ForMember(d => d.Product, o => o.Ignore())
        .ForMember(d => d.Unit, o => o.Ignore());
        #endregion
    }
}