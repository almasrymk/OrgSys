namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for resolving a single Invoice by id (e.g. Purchasing's PurchaseOrder linking
/// flow). Handled by CommercialDocuments.Application. Returns null when not found.
/// </summary>
public record GetInvoiceReferenceQuery(long Id) : IQuery<InvoiceReferenceDto?>;
