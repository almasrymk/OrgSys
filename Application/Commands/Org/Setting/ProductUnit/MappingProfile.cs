using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void ProductUnitMappingProfile()
    {
        #region ProductUnit
        CreateMap<ProductUnit, ProductUnitModelView>()
        .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name));
        CreateMap<ProductUnitModelView, ProductUnit>();       
        #endregion
    }
}