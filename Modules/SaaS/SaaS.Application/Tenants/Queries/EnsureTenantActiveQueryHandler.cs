namespace SaaS.Application.Tenants.Queries
{
    using OrgSys.SharedKernel;
    using SaaS.Contracts.Tenants;
    using System.Net;

    public sealed class EnsureTenantActiveQueryHandler(IRepository<Tenant> _Repository) : IQueryHandler<EnsureTenantActiveQuery, bool>
    {
        public async Task<Result<bool>> Handle(EnsureTenantActiveQuery request, CancellationToken cancellationToken)
        {
            var tenant = await _Repository.GetByFilterAsync(e => e.Id == request.TenantId, string.Empty);
            var isUsable = tenant is not null && tenant.TenantStatus is TenantLifecycleStatus.Trial or TenantLifecycleStatus.Active;
            return new Result<bool>(HttpStatusCode.OK, isUsable, null);
        }
    }
}
