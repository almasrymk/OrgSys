namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

public record GetInvoicePaymentInfoQuery(long Id) : IQuery<InvoicePaymentInfoDto?>;
