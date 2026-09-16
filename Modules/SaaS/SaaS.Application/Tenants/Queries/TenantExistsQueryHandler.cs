namespace SaaS.Application.Tenants.Queries;

using OrgSys.SharedKernel;
using SaaS.Contracts.Tenants;
using System.Net;

public sealed class TenantExistsQueryHandler(IRepository<Tenant> repository)
    : IQueryHandler<TenantExistsQuery, bool>
{
    public async Task<Result<bool>> Handle(TenantExistsQuery request, CancellationToken cancellationToken)
    {
        var exists = await repository.AnyAsync(e => e.Id == request.TenantId, cancellationToken);
        return new Result<bool>(HttpStatusCode.OK, exists, null);
    }
}
