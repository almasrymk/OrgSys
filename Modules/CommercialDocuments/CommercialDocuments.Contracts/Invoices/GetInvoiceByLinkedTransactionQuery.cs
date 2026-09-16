namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

public record GetInvoiceByLinkedTransactionQuery(long TransactionId) : IQuery<InvoiceReferenceDto?>;
