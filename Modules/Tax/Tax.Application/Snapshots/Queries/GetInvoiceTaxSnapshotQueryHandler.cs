namespace Tax.Application.Snapshots.Queries;

using Tax.Contracts.Snapshots;
using System.Net;

public sealed class GetInvoiceTaxSnapshotQueryHandler(IRepository<InvoiceTaxSnapshot> repository)
    : IQueryHandler<GetInvoiceTaxSnapshotQuery, InvoiceTaxSnapshotDto>
{
    public async Task<Result<InvoiceTaxSnapshotDto>> Handle(GetInvoiceTaxSnapshotQuery request, CancellationToken cancellationToken)
    {
        var snapshot = await repository.GetByFilterAsync(e => e.InvoiceId == request.InvoiceId, "Lines");
        if (snapshot is null || snapshot.Id == 0)
            return new Result<InvoiceTaxSnapshotDto>(HttpStatusCode.NotFound, null, [new Error("Tax snapshot not found.")]);

        var dto = new InvoiceTaxSnapshotDto(
            snapshot.Id,
            snapshot.InvoiceId,
            snapshot.InvoiceNumber,
            snapshot.InvoiceTypeId,
            snapshot.TaxType,
            snapshot.TaxInput,
            snapshot.TaxAmount,
            snapshot.Total,
            snapshot.Net,
            snapshot.CurrencyId,
            snapshot.CapturedAt,
            snapshot.SubmissionChannel.ToString(),
            snapshot.SubmissionStatus.ToString(),
            snapshot.ExternalReference,
            snapshot.Lines.Select(l => new InvoiceTaxSnapshotLineDto(
                l.ProductId, l.Quantity, l.Price, l.Tax, l.Net, l.Total)).ToList());

        return new Result<InvoiceTaxSnapshotDto>(HttpStatusCode.OK, dto, null);
    }
}
