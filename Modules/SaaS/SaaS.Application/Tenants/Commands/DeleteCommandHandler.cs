namespace SaaS.Application.Tenants.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteTenantCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Tenant> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTenantCommand, Tenant>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Tenant, bool>> CreateFilter(DeleteTenantCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }

    public sealed record DeleteListTenantCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Tenant> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTenantCommand, Tenant>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Tenant, bool>> CreateFilter(DeleteListTenantCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
