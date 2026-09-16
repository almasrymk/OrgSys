namespace CommercialDocuments.Application.Invoices.Queries;

using CommercialDocuments.Contracts.Invoices;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetInvoiceByLinkedTransactionQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> repository)
    : IQueryHandler<GetInvoiceByLinkedTransactionQuery, InvoiceReferenceDto?>
{
    public async Task<Result<InvoiceReferenceDto?>> Handle(GetInvoiceByLinkedTransactionQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByFilterAsync(e => e.TransactionId == request.TransactionId, string.Empty);
        if (invoice is null)
            return new Result<InvoiceReferenceDto?>(HttpStatusCode.OK, null, null);

        var dto = new InvoiceReferenceDto(
            invoice.Id,
            invoice.Code,
            (InvoiceTypeId)invoice.TypeId,
            invoice.DealerId,
            invoice.Status == Status.Deleted);

        return new Result<InvoiceReferenceDto?>(HttpStatusCode.OK, dto, null);
    }
}
