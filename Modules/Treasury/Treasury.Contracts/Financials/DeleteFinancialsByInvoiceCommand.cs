namespace Treasury.Contracts.Financials;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for "delete the Financial (unified cash/bank transaction) linked to a Sales
/// Invoice being deleted", via FinancialInvoices. No-op if none is linked. Handled by
/// Treasury.Application.
/// </summary>
public record DeleteFinancialsByInvoiceCommand(long InvoiceId) : ICommand;
