namespace SaaS.Application.Plans.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPlanQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<PlanDto>, ISearchQuery<ResultPagination<PlanDto>>;

    public sealed class SearchQueryHandler(IRepository<Plan> _Repository, IMapper mapper) : SearchCommandHandler<SearchPlanQuery, Plan, PlanDto>(_Repository, mapper)
    {
        public override Expression<Func<Plan, bool>> CreateFilter(SearchPlanQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Plan>, IOrderedQueryable<Plan>> CreateOrderBy(SearchPlanQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
