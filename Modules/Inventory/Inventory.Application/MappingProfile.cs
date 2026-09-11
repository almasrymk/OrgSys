namespace Inventory.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        ProductUnitMappingProfile();
        ProductMappingProfile();
        PropertyMappingProfile();
        StockMappingProfile();
        TransactionTypeMappingProfile();
        TransactionMappingProfile();
        InventoryMappingProfile();
    }
}
