namespace Application.Commands.Org.Setting.InvoiceType.Queries
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

    public sealed record SearchInvoiceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InvoiceTypeModelView> ,ISearchQuery<ResultPagination<InvoiceTypeModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.InvoiceType> _Repository, IMapper mapper) : SearchCommandHandler<SearchInvoiceTypeQuery, Entity.Model.InvoiceType, InvoiceTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.InvoiceType, bool>> CreateFilter(SearchInvoiceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.InvoiceType>, IOrderedQueryable<Entity.Model.InvoiceType>> CreateOrderBy(SearchInvoiceTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}