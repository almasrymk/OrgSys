namespace CommercialDocuments.Application.Invoices.Queries
{
    using Accounting.Contracts.Postings;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;
    using Catalog.Contracts.Products;

    public sealed record GetByIdInvoiceQuery(long Id) : ICommand<InvoiceDto> , IGetByIdQuery<Result<InvoiceDto>>;

    public sealed class GetByIdQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> _Repository, ISender sender, IMapper mapper) : GetCommandHandler<GetByIdInvoiceQuery, CommercialDocuments.Domain.Invoice, InvoiceDto>(_Repository, mapper)
    {
        public override async Task<Result<InvoiceDto>> Handle(GetByIdInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response == null)
                return result;

            var journal = (await sender.Send(new GetAccountingDocumentJournalQuery("invoice", result.Response.Id, result.Response.TypeId), cancellationToken)).Response;
            result.Response.JournalId = journal?.JournalId;
            result.Response.JournalCode = journal?.JournalCode;

            // InvoiceProduct.Product navigation was dropped (Sales/Inventory module boundary —
            // see docs/modular-monolith-analysis.md §21) — ProductName is now populated via a
            // batch lookup instead of an EF Include/flatten, same pattern as JournalCode above.
            var lineItems = result.Response.InvoiceProductList;
            if (lineItems is { Count: > 0 })
            {
                var productIds = lineItems.Select(e => e.ProductId).Distinct().ToList();
                var namesResult = await sender.Send(new GetProductNamesQuery(productIds), cancellationToken);
                var names = namesResult.Response ?? [];
                foreach (var line in lineItems)
                {
                    line.ProductName = names.GetValueOrDefault(line.ProductId);
                }
            }

            return result;
        }

        public override string CreateInclude()
        {
            // Product (and its chain into ProductUnits.Unit) and Transaction dropped: Product is
            // handled via the manual batch lookup above (module-boundary reasons); Transaction was
            // never actually read from the mapped DTO (confirmed dead —
            // docs/modular-monolith-analysis.md §21) so it is dropped outright.
            return "InvoiceProducts";
        }

        public override Expression<Func<CommercialDocuments.Domain.Invoice, bool>> CreateFilter(GetByIdInvoiceQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
