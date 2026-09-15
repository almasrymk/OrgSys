namespace Inventory.Application;

public class WarehouseLocationDto : BaseModel
{
    public long StockId { get; set; }
    public string? StockName { get; set; }
    public string? Name { get; set; }
    public long? ParentLocationId { get; set; }
    public string? LocationType { get; set; }
    public bool IsReceivable { get; set; } = true;
    public bool IsPickable { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
