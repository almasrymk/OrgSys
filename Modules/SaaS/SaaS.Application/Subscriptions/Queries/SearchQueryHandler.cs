namespace SaaS.Application.Subscriptions.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchSubscriptionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<SubscriptionDto>, ISearchQuery<ResultPagination<SubscriptionDto>>;

    public sealed class SearchQueryHandler(IRepository<Subscription> _Repository, IMapper mapper) : SearchCommandHandler<SearchSubscriptionQuery, Subscription, SubscriptionDto>(_Repository, mapper)
    {
        public override Expression<Func<Subscription, bool>> CreateFilter(SearchSubscriptionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (request.ParentId == 0 || e.TenantId == request.ParentId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Subscription>, IOrderedQueryable<Subscription>> CreateOrderBy(SearchSubscriptionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
