namespace Accounting.Application.Accounts.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetAccountNamesQuery — batch name lookup used by other modules instead of an EF Include across the module boundary. See docs/dependency-rules.md.</summary>
    public sealed class GetAccountNamesQueryHandler(IRepository<Accounting.Domain.Account> _Repository)
        : IQueryHandler<GetAccountNamesQuery, Dictionary<long, string?>>
    {
        public async Task<Result<Dictionary<long, string?>>> Handle(GetAccountNamesQuery request, CancellationToken cancellationToken)
        {
            if (request.AccountIds.Count == 0)
                return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

            var ids = request.AccountIds.Distinct().ToList();
            var accounts = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id));
            var names = (accounts ?? []).ToDictionary(e => e.Id, e => e.Name);

            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
        }
    }
}
