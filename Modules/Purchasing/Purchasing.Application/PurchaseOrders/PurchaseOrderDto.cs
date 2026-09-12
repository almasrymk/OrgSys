namespace Purchasing.Application
{
    public class PurchaseOrderDto : PurchaseOrder
    {
        public string? DealerName { get; set; }

        public string? CreateUserName { get; set; }

        public List<PurchaseOrderProductDto>? PurchaseOrderProductList { get; set; }
    }
}
