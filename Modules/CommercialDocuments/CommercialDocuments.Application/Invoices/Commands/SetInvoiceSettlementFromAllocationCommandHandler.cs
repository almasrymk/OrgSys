namespace CommercialDocuments.Application.Invoices.Commands;

using CommercialDocuments.Contracts.Invoices;
using OrgSys.SharedKernel;
using System.Net;

public sealed class SetInvoiceSettlementFromAllocationCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<CommercialDocuments.Domain.Invoice> repository)
    : ICommandHandler<SetInvoiceSettlementFromAllocationCommand>
{
    public async Task<Result> Handle(SetInvoiceSettlementFromAllocationCommand request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByFilterAsync(e => e.Id == request.InvoiceId, string.Empty);
        if (invoice is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Invoice not found")]);

        invoice.Credit = (invoice.Net - request.AllocatedAmount) - invoice.Paid;
        invoice.Paid = invoice.Net - invoice.Credit;
        await repository.UpdateAsync(invoice);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}
