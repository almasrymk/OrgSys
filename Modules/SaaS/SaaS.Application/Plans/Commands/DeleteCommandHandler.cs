namespace SaaS.Application.Plans.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeletePlanCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Plan> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePlanCommand, Plan>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Plan, bool>> CreateFilter(DeletePlanCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }

    public sealed record DeleteListPlanCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Plan> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPlanCommand, Plan>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Plan, bool>> CreateFilter(DeleteListPlanCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
