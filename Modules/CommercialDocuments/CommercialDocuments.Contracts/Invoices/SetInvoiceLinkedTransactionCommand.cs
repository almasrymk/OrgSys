namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

public record SetInvoiceLinkedTransactionCommand(long InvoiceId, long TransactionId) : ICommand;
