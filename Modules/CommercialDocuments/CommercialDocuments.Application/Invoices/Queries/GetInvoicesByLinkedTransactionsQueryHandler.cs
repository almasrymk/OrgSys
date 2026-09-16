namespace CommercialDocuments.Application.Invoices.Queries;

using CommercialDocuments.Contracts.Invoices;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetInvoicesByLinkedTransactionsQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> repository)
    : IQueryHandler<GetInvoicesByLinkedTransactionsQuery, Dictionary<long, InvoiceReferenceDto>>
{
    public async Task<Result<Dictionary<long, InvoiceReferenceDto>>> Handle(GetInvoicesByLinkedTransactionsQuery request, CancellationToken cancellationToken)
    {
        if (request.TransactionIds.Count == 0)
            return new Result<Dictionary<long, InvoiceReferenceDto>>(HttpStatusCode.OK, [], null);

        var ids = request.TransactionIds.Distinct().ToList();
        var invoices = await repository.GetListByFilterAsync(
            e => e.TransactionId.HasValue && ids.Contains(e.TransactionId.Value));

        var map = (invoices ?? [])
            .Where(e => e.TransactionId.HasValue)
            .GroupBy(e => e.TransactionId!.Value)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var invoice = g.First();
                    return new InvoiceReferenceDto(
                        invoice.Id,
                        invoice.Code,
                        (InvoiceTypeId)invoice.TypeId,
                        invoice.DealerId,
                        invoice.Status == Status.Deleted);
                });

        return new Result<Dictionary<long, InvoiceReferenceDto>>(HttpStatusCode.OK, map, null);
    }
}
