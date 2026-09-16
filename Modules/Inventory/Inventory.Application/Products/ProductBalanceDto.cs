namespace Inventory.Application
{
    public class ProductBalanceDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Nickname { get; set; }
        public string? Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public long ClassificationId { get; set; }
        public string? ClassificationName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
        public IReadOnlyList<Catalog.Contracts.Products.ProductUnitLookupDto>? ProductUnits { get; set; }
    }
}
