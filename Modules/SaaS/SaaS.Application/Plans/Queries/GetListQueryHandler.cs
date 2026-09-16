namespace SaaS.Application.Plans.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListPlanQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<PlanDto>, IListQuery<ResultCollection<PlanDto>>;

    public sealed class GetListQueryHandler(IRepository<Plan> _Repository, IMapper mapper) : ListCommandHandler<GetListPlanQuery, Plan, PlanDto>(_Repository, mapper)
    {
        public override Expression<Func<Plan, bool>> CreateFilter(GetListPlanQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Plan>, IOrderedQueryable<Plan>> CreateOrderBy(GetListPlanQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
