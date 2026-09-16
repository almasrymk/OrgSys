namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

/// <summary>
/// Batch lookup of Invoice.Net by id — used by Treasury FinancialInvoice lines after the
/// FinancialInvoice.Invoice navigation was dropped.
/// </summary>
public record GetInvoiceNetsQuery(IReadOnlyCollection<long> InvoiceIds) : IQuery<Dictionary<long, decimal>>;
