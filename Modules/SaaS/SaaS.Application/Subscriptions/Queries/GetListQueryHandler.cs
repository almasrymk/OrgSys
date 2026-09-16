namespace SaaS.Application.Subscriptions.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListSubscriptionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<SubscriptionDto>, IListQuery<ResultCollection<SubscriptionDto>>;

    public sealed class GetListQueryHandler(IRepository<Subscription> _Repository, IMapper mapper) : ListCommandHandler<GetListSubscriptionQuery, Subscription, SubscriptionDto>(_Repository, mapper)
    {
        public override Expression<Func<Subscription, bool>> CreateFilter(GetListSubscriptionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            // ParentId is repurposed as an optional TenantId filter — same "generic filter slots"
            // convention the rest of this codebase's ListQuery records already use.
            return e =>
            (request.ParentId == 0 || e.TenantId == request.ParentId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Subscription>, IOrderedQueryable<Subscription>> CreateOrderBy(GetListSubscriptionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
