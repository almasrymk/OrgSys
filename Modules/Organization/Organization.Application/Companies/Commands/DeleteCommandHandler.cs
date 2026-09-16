namespace Organization.Application.Companies.Commands
{
    using OrgSys.SharedKernel;
    using SaaS.Contracts.Tenancy;
    using System.Linq.Expressions;

    public sealed record DeleteCompanyCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Organization.Domain.Company> _Repository,
        IServiceProvider _provider,
        ICurrentTenant currentTenant) : DeleteCommandHandler<DeleteCompanyCommand, Organization.Domain.Company>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Organization.Domain.Company, bool>> CreateFilter(DeleteCompanyCommand request)
        {
            if (currentTenant.TenantId is long tenantId)
                return e => e.Id == request.Id && e.TenantId == tenantId && e.Status != Status.Deleted && e.Hide != true;

            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}
