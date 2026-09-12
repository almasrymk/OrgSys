namespace Parties.Application.DealerGroups.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListDealerGroupQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DealerGroupDto> , IListQuery<ResultCollection<DealerGroupDto>>;

    public sealed class GetListQueryHandler(IRepository<Parties.Domain.DealerGroup> _Repository, IMapper mapper) : ListCommandHandler<GetListDealerGroupQuery, Parties.Domain.DealerGroup, DealerGroupDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.DealerGroup, bool>> CreateFilter(GetListDealerGroupQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Parties.Domain.DealerGroup>, IOrderedQueryable<Parties.Domain.DealerGroup>> CreateOrderBy(GetListDealerGroupQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}