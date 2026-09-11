namespace Sales.Application.Invoices.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;
    using Inventory.Contracts.Stocks;

    public sealed record SearchInvoiceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InvoiceDto> ,ISearchQuery<ResultPagination<InvoiceDto>>;

    public sealed class SearchQueryHandler(IRepository<Sales.Domain.Invoice> _Repository, IRepository<Accounting.Domain.Journal> journalRepository, ISender sender, IMapper mapper) : SearchCommandHandler<SearchInvoiceQuery, Sales.Domain.Invoice, InvoiceDto>(_Repository, mapper)
    {
        public override async Task<ResultPagination<InvoiceDto>> Handle(SearchInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var invoiceIds = result.Response.Select(e => e.Id).ToList();
            if (invoiceIds.Count == 0)
                return result;

            var journals = await journalRepository.GetListByFilterAsync(
                e => e.RefranceTable == "invoice" && invoiceIds.Contains(e.RefranceId));
            foreach (var invoice in result.Response)
            {
                var journal = journals?.FirstOrDefault(e => e.RefranceId == invoice.Id && e.RefranceTypeId == invoice.TypeId);
                invoice.JournalId = journal?.Id;
                invoice.JournalCode = journal?.Code;
            }

            // Invoice.Stock navigation was dropped (Sales/Inventory module boundary — see
            // docs/modular-monolith-analysis.md §21) — StockName is now populated the same way
            // JournalCode already was, via a batch lookup instead of an EF Include/flatten.
            var stockIds = result.Response.Where(e => e.StockId.HasValue).Select(e => e.StockId!.Value).Distinct().ToList();
            if (stockIds.Count > 0)
            {
                var namesResult = await sender.Send(new GetStockNamesQuery(stockIds), cancellationToken);
                var names = namesResult.Response ?? [];
                foreach (var invoice in result.Response)
                {
                    invoice.StockName = invoice.StockId.HasValue ? names.GetValueOrDefault(invoice.StockId.Value) : null;
                }
            }

            return result;
        }

        public override Expression<Func<Sales.Domain.Invoice, bool>> CreateFilter(SearchInvoiceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            // Stock and Transaction dropped: Stock is handled via the manual batch lookup above
            // (module-boundary reasons); Transaction was never actually read from the mapped DTO
            // (confirmed dead — docs/modular-monolith-analysis.md §21) so it is dropped outright.
            return "Dealer,PaymentType,Currency";
        }

        override public Func<IQueryable<Sales.Domain.Invoice>, IOrderedQueryable<Sales.Domain.Invoice>> CreateOrderBy(SearchInvoiceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
