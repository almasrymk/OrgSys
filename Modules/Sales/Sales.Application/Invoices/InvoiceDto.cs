
namespace Sales.Application
{
    public class InvoiceDto : Invoice
    {     
        public string? DealerName { get; set; }

        public string? PaymentTypeName { get; set; }

        public string? StockName { get; set; }

        public string? ParentCode { get; set; }

        public string? CurrencyName { get; set; }

        public string?   BranchName { get; set; }

        public string? CreateUserName { get; set; }

        public string? ModifyUserName { get; set; }

        public string? ShiftName { get; set; }

        public bool Cash { get; set; }      

        public long? JournalId { get; set; }

        public string? JournalCode { get; set; }

        public List<InvoiceProductDto>? InvoiceProductList { get; set; }
    }
}
