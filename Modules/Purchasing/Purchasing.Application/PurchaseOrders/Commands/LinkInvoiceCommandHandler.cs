namespace Purchasing.Application.PurchaseOrders.Commands
{
    using CommercialDocuments.Contracts.Invoices;
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    /// <summary>
    /// Manually links an already-created Purchase Invoice (CommercialDocuments.Domain.Invoice,
    /// TypeId 2/4 — same "Purchase"/"Purchase Return" discriminator InvoiceJournalIntegration
    /// already branches on) to a PurchaseOrder, marking the order Approved. No automatic Invoice
    /// creation happens anywhere in this module — the Invoice is created the normal way (via
    /// CommercialDocuments' own Invoice endpoints) and then linked here. Resolves the invoice
    /// through CommercialDocuments.Contracts (not IRepository&lt;Invoice&gt;) so this module has
    /// no dependency on CommercialDocuments.Domain, let alone Sales.Domain. The actual link/status
    /// transition is PurchaseOrder.LinkInvoice's own invariant now (idempotency guard included),
    /// not this handler's.
    /// </summary>
    public sealed record LinkInvoiceCommand(long PurchaseOrderId, long InvoiceId) : IRequest<Result>;

    public sealed class LinkInvoiceCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<PurchaseOrder> orderRepository,
        ISender sender) : IRequestHandler<LinkInvoiceCommand, Result>
    {
        public async Task<Result> Handle(LinkInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByFilterAsync(e => e.Id == request.PurchaseOrderId, string.Empty);
            if (order is null || order.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase order not found.")]);

            if (order.InvoiceId is > 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("This purchase order is already linked to an invoice.")]);

            var invoiceResult = await sender.Send(new GetInvoiceReferenceQuery(request.InvoiceId), cancellationToken);
            var invoice = invoiceResult.Response;
            if (invoice is null || invoice.IsDeleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Invoice not found.")]);

            if (invoice.TypeId is not (InvoiceTypeId.Purchase or InvoiceTypeId.PurchaseReturn))
                return new Result(HttpStatusCode.BadRequest, [new Error("The linked invoice must be a Purchase invoice or Purchase return.")]);

            try
            {
                order.LinkInvoice(invoice.Id);
            }
            catch (PurchaseOrderDomainException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            await orderRepository.UpdateAsync(order);
            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

            return new Result(HttpStatusCode.OK, null);
        }
    }
}
