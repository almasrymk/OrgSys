using Domain.Entities;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class FinancialDto : Financial
    {      

        public string? DealerName { get; set; }

        public string? PaymentTypeName { get; set; }

        public string? OutlayName { get; set; }

        public string? CashBoxName { get; set; }

        public string? CurrencyName { get; set; }

        public string? FinancialTypeName { get; set; }

        public string? FinancialAccountName { get; set; }

        public string? ContraFinancialAccountName { get; set; }

        public long CounterAccountId { get; set; }

        // Opening Balance (FinancialTypeId 1) only — view-only fields, never persisted on the Financial
        // entity/table. Opening Balance doesn't create a Financial row at all; it writes a JournalItem line
        // into the shared per-fiscal-year Opening Balance Journal via SetFinancialAccountOpeningBalanceCommand,
        // the same mechanism SetCustomerOpeningBalanceCommand/SetSupplierOpeningBalanceCommand already use.
        public long? FiscalYearId { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public List<FinancialInvoiceDto>? FinancialInvoiceList { get; set; }
    }
}
