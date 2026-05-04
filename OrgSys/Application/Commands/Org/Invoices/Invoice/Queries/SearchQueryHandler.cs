namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchInvoiceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InvoiceModelView> ,ISearchQuery<ResultPagination<InvoiceModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Invoice> _Repository, IMapper mapper) : SearchCommandHandler<SearchInvoiceQuery, Entity.Model.Invoice, InvoiceModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Invoice, bool>> CreateFilter(SearchInvoiceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Stock,PaymentType,Currency";
        }

        override public Func<IQueryable<Entity.Model.Invoice>, IOrderedQueryable<Entity.Model.Invoice>> CreateOrderBy(SearchInvoiceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}