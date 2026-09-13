namespace Accounting.Application.Accounts.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetAccountLookupsQuery — batch AccountLookupDto lookup used by other modules instead of an EF Include across the module boundary. See docs/dependency-rules.md.</summary>
    public sealed class GetAccountLookupsQueryHandler(IRepository<Accounting.Domain.Account> _Repository)
        : IQueryHandler<GetAccountLookupsQuery, Dictionary<long, AccountLookupDto>>
    {
        public async Task<Result<Dictionary<long, AccountLookupDto>>> Handle(GetAccountLookupsQuery request, CancellationToken cancellationToken)
        {
            if (request.AccountIds.Count == 0)
                return new Result<Dictionary<long, AccountLookupDto>>(HttpStatusCode.OK, [], null);

            var ids = request.AccountIds.Distinct().ToList();
            var accounts = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id), "AccountType");
            var lookups = (accounts ?? []).ToDictionary(e => e.Id, GetAccountQueryHandler.ToDto);

            return new Result<Dictionary<long, AccountLookupDto>>(HttpStatusCode.OK, lookups, null);
        }
    }
}
