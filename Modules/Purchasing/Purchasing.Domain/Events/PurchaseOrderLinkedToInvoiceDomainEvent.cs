namespace Purchasing.Domain.Events;

/// <summary>Raised by PurchaseOrder.LinkInvoice — mirrors the existing LinkInvoiceCommandHandler's
/// effect (brief §40: Purchasing never creates the invoice itself, only records the link once
/// CommercialDocuments/AccountsPayable's own posting has already happened).</summary>
public sealed record PurchaseOrderLinkedToInvoiceDomainEvent(long PurchaseOrderId, long InvoiceId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
