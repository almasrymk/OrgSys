namespace SaaS.Application.Plans.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreatePlanCommand : PlanDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Plan> _Repository, IMapper mapper) : CreateCommandHandler<CreatePlanCommand, Plan>(_UnitOfWork, _Repository, mapper)
    {
    }
}
