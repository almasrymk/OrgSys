namespace Receivables.Application.Balances.Queries
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using MediatR;
    using OrgSys.SharedKernel;
    using Receivables.Contracts.Balances;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetCustomerBalanceQuery — Receivables' own "Outstanding
    /// Balance" view of a customer, computed the same way as Parties' generic
    /// GetDealerBalanceQuery (Sum(Debit-Credit) of valid Journal history against the customer's
    /// receivable account) but resolved/validated through IReceivableAccountValidator so this
    /// module owns the AR-specific meaning of "balance" per its own Contracts surface. Ledger
    /// activity itself is read through Accounting.Contracts.Postings.GetAccountActivityQuery instead
    /// of an IRepository&lt;JournalItem&gt; reference across the module boundary.
    /// </summary>
    public sealed class GetCustomerBalanceQueryHandler(
        IReceivableAccountValidator _Validator,
        ISender sender) : IQueryHandler<GetCustomerBalanceQuery, decimal>
    {
        public async Task<Result<decimal>> Handle(GetCustomerBalanceQuery request, CancellationToken cancellationToken)
        {
            var (dealer, _, errors) = await _Validator.ValidateCustomerAsync(request.DealerId, cancellationToken);
            if (errors.Count > 0 || dealer?.AccountId is not > 0)
                return new Result<decimal>(HttpStatusCode.BadRequest, 0, errors.Count > 0 ? errors : [new Error("Customer does not have a linked receivable account.")]);

            var items = (await sender.Send(new GetAccountActivityQuery(dealer.AccountId.Value, request.AsOfDate), cancellationToken)).Response ?? [];

            var balance = items.Sum(e => e.Debit - e.Credit);
            return new Result<decimal>(HttpStatusCode.OK, balance, null);
        }
    }
}
