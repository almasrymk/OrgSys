namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

/// <summary>
/// Owner-side mutation of Invoice.Paid/Credit. Treasury must not write Invoice rows.
/// Positive paidDelta applies a payment; negative reverses one.
/// </summary>
public record AdjustInvoiceSettlementCommand(long InvoiceId, decimal PaidDelta) : ICommand;
