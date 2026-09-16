namespace SaaS.Application.Tenants.Queries
{
    using OrgSys.SharedKernel;
    using SaaS.Contracts.Tenants;
    using System.Net;

    /// <summary>Resolves the single seeded "Default Tenant" — used by the tenant retrofit's
    /// backfill step (docs/architecture/adr/tenant-vs-company.md) so existing rows get assigned a
    /// real TenantId without hardcoding one. Mirrors
    /// Organization.Application.Companies.Queries.GetDefaultCompanyQueryHandler.</summary>
    public sealed class GetDefaultTenantQueryHandler(IRepository<Tenant> _Repository) : IQueryHandler<GetDefaultTenantQuery, TenantLookupDto?>
    {
        public async Task<Result<TenantLookupDto?>> Handle(GetDefaultTenantQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _Repository.GetListByFilterAsync(e => e.Hide != true);
            var tenant = tenants?.OrderBy(e => e.Id).FirstOrDefault();

            var dto = tenant is null ? null : new TenantLookupDto(tenant.Id, tenant.Name, tenant.TenantStatus.ToString(), tenant.Hide != true);
            return new Result<TenantLookupDto?>(HttpStatusCode.OK, dto, null);
        }
    }
}
