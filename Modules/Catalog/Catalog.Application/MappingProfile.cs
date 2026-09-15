namespace Catalog.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        ProductMappingProfile();
        ProductUnitMappingProfile();
        PropertyMappingProfile();
        ClassificationMappingProfile();
        UnitMappingProfile();
        BrandMappingProfile();
        PriceListMappingProfile();
    }
}
