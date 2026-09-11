namespace Treasury.Application.Outlays.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListOutlayQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<OutlayDto> , IListQuery<ResultCollection<OutlayDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.Outlay> _Repository, IMapper mapper) : ListCommandHandler<GetListOutlayQuery, Treasury.Domain.Outlay, OutlayDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Outlay, bool>> CreateFilter(GetListOutlayQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Treasury.Domain.Outlay>, IOrderedQueryable<Treasury.Domain.Outlay>> CreateOrderBy(GetListOutlayQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}