namespace CommercialDocuments.Application.Invoices.Queries;

using CommercialDocuments.Contracts.Invoices;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetInvoiceInventoryImpactQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> repository)
    : IQueryHandler<GetInvoiceInventoryImpactQuery, InvoiceInventoryImpactDto?>
{
    public async Task<Result<InvoiceInventoryImpactDto?>> Handle(GetInvoiceInventoryImpactQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByFilterAsync(e => e.Id == request.InvoiceId, "InvoiceProducts");
        if (invoice is null)
            return new Result<InvoiceInventoryImpactDto?>(HttpStatusCode.OK, null, null);

        var lines = (invoice.InvoiceProducts ?? [])
            .Select(e => new InvoiceInventoryLineDto(e.ProductId, e.UnitId, e.StockId, e.Quantity, e.Price, e.Notes))
            .ToList();

        var dto = new InvoiceInventoryImpactDto(
            invoice.Id,
            invoice.Code,
            invoice.TypeId,
            invoice.DealerId,
            invoice.StockId,
            invoice.TransactionId,
            invoice.Date,
            invoice.CreateDate,
            invoice.CreateUserId,
            invoice.ShiftId,
            invoice.BranchId,
            invoice.Notes,
            invoice.Posted,
            lines);

        return new Result<InvoiceInventoryImpactDto?>(HttpStatusCode.OK, dto, null);
    }
}
