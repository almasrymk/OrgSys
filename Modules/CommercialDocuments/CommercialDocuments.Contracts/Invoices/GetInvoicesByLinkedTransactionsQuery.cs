namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

/// <summary>Keyed by TransactionId so Inventory can enrich a page of transactions.</summary>
public record GetInvoicesByLinkedTransactionsQuery(IReadOnlyCollection<long> TransactionIds)
    : IQuery<Dictionary<long, InvoiceReferenceDto>>;
