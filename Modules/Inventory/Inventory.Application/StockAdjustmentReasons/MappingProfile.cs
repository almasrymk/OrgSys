namespace Inventory.Application;

using Inventory.Application.StockAdjustmentReasons.Commands;

public partial class MappingProfile
{
    public void StockAdjustmentReasonMappingProfile()
    {
        CreateMap<StockAdjustmentReason, StockAdjustmentReasonDto>();
        CreateMap<StockAdjustmentReasonDto, StockAdjustmentReason>();

        CreateMap<CreateStockAdjustmentReasonCommand, StockAdjustmentReason>();
        CreateMap<StockAdjustmentReason, CreateStockAdjustmentReasonCommand>();
    }
}
