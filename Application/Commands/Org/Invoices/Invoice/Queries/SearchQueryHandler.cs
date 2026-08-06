namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchInvoiceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InvoiceDto> ,ISearchQuery<ResultPagination<InvoiceDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Invoice> _Repository, IRepository<Domain.Entities.Journal> journalRepository, IMapper mapper) : SearchCommandHandler<SearchInvoiceQuery, Domain.Entities.Invoice, InvoiceDto>(_Repository, mapper)
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

            return result;
        }

        public override Expression<Func<Domain.Entities.Invoice, bool>> CreateFilter(SearchInvoiceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Stock,PaymentType,Currency,Transaction";
        }

        override public Func<IQueryable<Domain.Entities.Invoice>, IOrderedQueryable<Domain.Entities.Invoice>> CreateOrderBy(SearchInvoiceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
