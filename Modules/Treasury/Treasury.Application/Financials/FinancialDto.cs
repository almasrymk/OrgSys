using System.Collections.Generic;

namespace Treasury.Application
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

        public string? CounterAccountName { get; set; }

        // Display text for the currently-selected ReferenceId on edit (Droptxt autocomplete needs a
        // label, not just the raw Id) — resolved per-ReferenceType in FinancialController.LoadViewBag,
        // same idea as FinancialAccountName/CounterAccountName above.
        public string? ReferenceName { get; set; }

        // Opening Balance (FinancialTypeId 1) only — a view-only convenience for picking a fiscal year in
        // the UI, never persisted on the Financial entity/table. The actual fiscal year/period is resolved
        // from Date (via IAccountingPeriodService) at Post time, exactly like every other Financial type;
        // this just drives the dropdown and defaults Date to that year's start date (see FinancialController).
        public long? FiscalYearId { get; set; }

        // List.cshtml's row actions mirror Journal's List.cshtml, which distinguishes a Journal
        // created FROM a source screen (RefranceTable == "invoice"/"transaction", read-only, follow the
        // source instead) from a manually created one (RefranceTable == "", standard Edit/Cancel/Redo/
        // Delete). No Financial-row creation path (PostTransactionCommandHandler, the generic Create/
        // Update handlers, PostFinancialTransferCommandHandler, PostFinancialOpeningBalanceCommandHandler)
        // ever sets a polymorphic source-table indicator on Financial the way Journal does — every
        // Financial row is the manually-created case. Stubbed so the view compiles against fields that
        // never existed on this entity, and always resolves to that "" case.
        public string? RefranceTable => null;

        public long RefranceTypeId => 0;

        public long RefranceId => 0;

        public string? RefranceCode => null;

        public List<FinancialInvoiceDto>? FinancialInvoiceList { get; set; }
    }
}
