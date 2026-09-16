namespace Organization.Application.Branches.Queries;

using OrgSys.SharedKernel;
using Organization.Contracts.Branches;
using System.Net;

/// <summary>Handles the Contracts-facing GetBranchNamesQuery — batch name lookup used by other
/// modules instead of an EF Include across the module boundary.</summary>
public sealed class GetBranchNamesQueryHandler(IRepository<Organization.Domain.Branch> _Repository)
    : IQueryHandler<GetBranchNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetBranchNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.BranchIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.BranchIds.Distinct().ToList();
        var branches = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (branches ?? []).ToDictionary(e => e.Id, e => e.Name);

        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
