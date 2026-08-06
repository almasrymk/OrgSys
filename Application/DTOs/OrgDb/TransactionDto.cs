using Domain.Entities;

namespace Application.DTOs
{
    public class TransactionDto : Transaction
    {
        public string? DealerName { get; set; }

        public string? StockName { get; set; }

        public string? ToStockName { get; set; }

        public string? ParentCode { get; set; }

        public string? BranchName { get; set; }

        public string? CreateUserName { get; set; }

        public string? ModifyUserName { get; set; }

        public string? ShiftName { get; set; }

        public long? SourceInvoiceId { get; set; }

        public string? SourceInvoiceCode { get; set; }

        public long? SourceInvoiceTypeId { get; set; }

        public List<TransactionProductDto>? TransactionProductList { get; set; }
    }
}
