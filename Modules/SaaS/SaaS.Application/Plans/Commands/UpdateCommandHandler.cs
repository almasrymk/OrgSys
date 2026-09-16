namespace SaaS.Application.Plans.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePlanCommand : PlanDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Plan> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePlanCommand, Plan>(_UnitOfWork, _Repository, mapper, _provider)
    {
    }
}
