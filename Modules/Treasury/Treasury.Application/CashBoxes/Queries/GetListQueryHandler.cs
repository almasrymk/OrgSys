namespace Treasury.Application.CashBoxes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListCashBoxQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<CashBoxDto>, IListQuery<ResultCollection<CashBoxDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.CashBox> _Repository, IMapper mapper) : ListCommandHandler<GetListCashBoxQuery, Treasury.Domain.CashBox, CashBoxDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.CashBox, bool>> CreateFilter(GetListCashBoxQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Treasury.Domain.CashBox>, IOrderedQueryable<Treasury.Domain.CashBox>> CreateOrderBy(GetListCashBoxQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
