using Domain.Entities;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class FinancialDto : Financial
    {      

        public string? DealerName { get; set; }

        public string? PaymentTypeName { get; set; }

        public string? OutlayName { get; set; }

        public string? SafeName { get; set; }

        public string? CurrencyName { get; set; }

        public string? FinancialTypeName { get; set; }

        public string? FinancialAccountName { get; set; }

        public string? ContraFinancialAccountName { get; set; }

        public long CounterAccountId { get; set; }

        public List<FinancialInvoiceDto>? FinancialInvoiceList { get; set; }
    }
}
