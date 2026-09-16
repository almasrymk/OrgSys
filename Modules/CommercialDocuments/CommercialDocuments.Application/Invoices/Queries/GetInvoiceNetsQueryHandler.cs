namespace CommercialDocuments.Application.Invoices.Queries;

using CommercialDocuments.Contracts.Invoices;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetInvoiceNetsQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> repository)
    : IQueryHandler<GetInvoiceNetsQuery, Dictionary<long, decimal>>
{
    public async Task<Result<Dictionary<long, decimal>>> Handle(GetInvoiceNetsQuery request, CancellationToken cancellationToken)
    {
        if (request.InvoiceIds.Count == 0)
            return new Result<Dictionary<long, decimal>>(HttpStatusCode.OK, [], null);

        var ids = request.InvoiceIds.Distinct().ToList();
        var invoices = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var nets = (invoices ?? []).ToDictionary(e => e.Id, e => e.Net);

        return new Result<Dictionary<long, decimal>>(HttpStatusCode.OK, nets, null);
    }
}
