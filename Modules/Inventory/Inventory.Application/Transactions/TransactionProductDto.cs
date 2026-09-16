namespace Inventory.Application
{
    public class TransactionProductDto : TransactionProduct
    {       
        public string? ProductName { get; set; }

        public string? UnitName { get; set; }

        public string? StockName { get; set; }

        public List<UnitNameDto>? UnitList { get; set; }
    }
}
