namespace CommercialDocuments.Application.InvoiceTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchInvoiceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InvoiceTypeDto> ,ISearchQuery<ResultPagination<InvoiceTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<CommercialDocuments.Domain.InvoiceType> _Repository, IMapper mapper) : SearchCommandHandler<SearchInvoiceTypeQuery, CommercialDocuments.Domain.InvoiceType, InvoiceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<CommercialDocuments.Domain.InvoiceType, bool>> CreateFilter(SearchInvoiceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<CommercialDocuments.Domain.InvoiceType>, IOrderedQueryable<CommercialDocuments.Domain.InvoiceType>> CreateOrderBy(SearchInvoiceTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}