namespace Accounting.Application.Accounts.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetAccountQuery — a single Account's public shape, used instead of an IRepository&lt;Account&gt; reference across the module boundary. See docs/dependency-rules.md.</summary>
    public sealed class GetAccountQueryHandler(IRepository<Accounting.Domain.Account> _Repository)
        : IQueryHandler<GetAccountQuery, AccountLookupDto?>
    {
        public async Task<Result<AccountLookupDto?>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _Repository.GetByFilterAsync(e => e.Id == request.AccountId, "AccountType");
            if (account is null)
                return new Result<AccountLookupDto?>(HttpStatusCode.OK, null, null);

            return new Result<AccountLookupDto?>(HttpStatusCode.OK, ToDto(account), null);
        }

        internal static AccountLookupDto ToDto(Accounting.Domain.Account account) => new(
            account.Id, account.Name, account.Code, account.AccountTypeId, account.AccountType?.Name,
            account.IsPostable, account.Status != Status.Deleted && !account.Hide);
    }
}
