namespace SaaS.Application.Plans.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPlanQuery(long Id) : ICommand<PlanDto>, IGetByIdQuery<Result<PlanDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Plan> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPlanQuery, Plan, PlanDto>(_Repository, mapper)
    {
        public override Expression<Func<Plan, bool>> CreateFilter(GetByIdPlanQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
