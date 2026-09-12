namespace Purchasing.Application.PurchaseOrders.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>
    /// Manually links an already-created Purchase Invoice (Sales.Domain.Invoice, TypeId 2/4 —
    /// same "Purchase"/"Purchase Return" discriminator InvoiceJournalIntegration already branches
    /// on) to a PurchaseOrder, marking the order Approved. No automatic Invoice creation happens
    /// anywhere in this module — the Invoice is created the normal way (Sales' own Invoice
    /// endpoints) and then linked here.
    /// </summary>
    public sealed record LinkInvoiceCommand(long PurchaseOrderId, long InvoiceId) : IRequest<Result>;

    public sealed class LinkInvoiceCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<PurchaseOrder> _OrderRepository,
        IRepository<Sales.Domain.Invoice> _InvoiceRepository) : IRequestHandler<LinkInvoiceCommand, Result>
    {
        public async Task<Result> Handle(LinkInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await _OrderRepository.GetByFilterAsync(e => e.Id == request.PurchaseOrderId, string.Empty);
            if (order is null || order.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase order not found.")]);

            if (order.InvoiceId is > 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("This purchase order is already linked to an invoice.")]);

            var invoice = await _InvoiceRepository.GetByFilterAsync(e => e.Id == request.InvoiceId, string.Empty);
            if (invoice is null || invoice.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Invoice not found.")]);

            if (invoice.TypeId is not (2 or 4))
                return new Result(HttpStatusCode.BadRequest, [new Error("The linked invoice must be a Purchase invoice or Purchase return.")]);

            order.InvoiceId = invoice.Id;
            order.Status = Status.Approved;
            await _OrderRepository.UpdateAsync(order);
            if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

            return new Result(HttpStatusCode.OK, null);
        }
    }
}
