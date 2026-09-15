namespace Inventory.Application;

public class StockAdjustmentReasonDto : BaseModel
{
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
}
