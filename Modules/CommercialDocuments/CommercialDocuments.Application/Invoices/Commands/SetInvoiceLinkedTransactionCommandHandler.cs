namespace CommercialDocuments.Application.Invoices.Commands;

using CommercialDocuments.Contracts.Invoices;
using OrgSys.SharedKernel;
using System.Net;

public sealed class SetInvoiceLinkedTransactionCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<CommercialDocuments.Domain.Invoice> repository)
    : ICommandHandler<SetInvoiceLinkedTransactionCommand>
{
    public async Task<Result> Handle(SetInvoiceLinkedTransactionCommand request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByFilterAsync(e => e.Id == request.InvoiceId, string.Empty);
        if (invoice is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Invoice not found")]);

        invoice.TransactionId = request.TransactionId;
        await repository.UpdateAsync(invoice);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}
