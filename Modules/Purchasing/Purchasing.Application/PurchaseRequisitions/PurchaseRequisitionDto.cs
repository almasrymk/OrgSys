namespace Purchasing.Application
{
    public class PurchaseRequisitionDto : PurchaseRequisition
    {
        public string? CreateUserName { get; set; }

        public List<PurchaseRequisitionProductDto>? PurchaseRequisitionProductList { get; set; }
    }
}
