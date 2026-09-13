namespace CommercialDocuments.Application.Invoices.Queries
{
    using CommercialDocuments.Contracts.Invoices;
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetInvoiceReferenceQuery — used by other modules (e.g.
    /// Purchasing's PurchaseOrder linking flow) instead of an IRepository&lt;Invoice&gt;
    /// injection across the module boundary. See docs/commercial-documents-module.md.
    /// </summary>
    public sealed class GetInvoiceReferenceQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> _Repository)
        : IQueryHandler<GetInvoiceReferenceQuery, InvoiceReferenceDto?>
    {
        public async Task<Result<InvoiceReferenceDto?>> Handle(GetInvoiceReferenceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
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
}
