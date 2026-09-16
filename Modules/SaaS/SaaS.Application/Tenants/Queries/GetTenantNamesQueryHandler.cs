namespace SaaS.Application.Tenants.Queries
{
    using OrgSys.SharedKernel;
    using SaaS.Contracts.Tenants;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetTenantNamesQuery — batch name lookup used by other
    /// modules instead of an EF reference across the module boundary. Mirrors
    /// Organization.Application.Companies.Queries.GetCompanyNamesQueryHandler.</summary>
    public sealed class GetTenantNamesQueryHandler(IRepository<Tenant> _Repository) : IQueryHandler<GetTenantNamesQuery, Dictionary<long, string?>>
    {
        public async Task<Result<Dictionary<long, string?>>> Handle(GetTenantNamesQuery request, CancellationToken cancellationToken)
        {
            if (request.TenantIds.Count == 0)
                return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

            var ids = request.TenantIds.Distinct().ToList();
            var tenants = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id));
            var names = (tenants ?? []).ToDictionary(e => e.Id, e => (string?)e.Name);

            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
        }
    }
}
