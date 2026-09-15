namespace Inventory.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        StockMappingProfile();
        TransactionTypeMappingProfile();
        TransactionMappingProfile();
        InventoryMappingProfile();
        WarehouseLocationMappingProfile();
        StockAdjustmentReasonMappingProfile();
    }
}
