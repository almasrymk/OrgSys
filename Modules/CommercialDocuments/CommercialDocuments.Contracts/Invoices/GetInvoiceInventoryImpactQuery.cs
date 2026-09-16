namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

public record GetInvoiceInventoryImpactQuery(long InvoiceId) : IQuery<InvoiceInventoryImpactDto?>;
