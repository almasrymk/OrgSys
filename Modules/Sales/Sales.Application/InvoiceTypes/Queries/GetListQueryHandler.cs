namespace Sales.Application.InvoiceTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListInvoiceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<InvoiceTypeDto> , IListQuery<ResultCollection<InvoiceTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Sales.Domain.InvoiceType> _Repository, IMapper mapper) : ListCommandHandler<GetListInvoiceTypeQuery, Sales.Domain.InvoiceType, InvoiceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Sales.Domain.InvoiceType, bool>> CreateFilter(GetListInvoiceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Sales.Domain.InvoiceType>, IOrderedQueryable<Sales.Domain.InvoiceType>> CreateOrderBy(GetListInvoiceTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}