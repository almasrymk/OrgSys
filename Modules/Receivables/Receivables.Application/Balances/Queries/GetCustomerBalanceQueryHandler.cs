namespace Receivables.Application.Balances.Queries
{
    using OrgSys.SharedKernel;
    using Accounting.Application;
    using Receivables.Contracts.Balances;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetCustomerBalanceQuery — Receivables' own "Outstanding
    /// Balance" view of a customer, computed the same way as Parties' generic
    /// GetDealerBalanceQuery (Sum(Debit-Credit) of valid Journal history against the customer's
    /// receivable account) but resolved/validated through IReceivableAccountValidator so this
    /// module owns the AR-specific meaning of "balance" per its own Contracts surface.
    /// </summary>
    public sealed class GetCustomerBalanceQueryHandler(
        IReceivableAccountValidator _Validator,
        IRepository<JournalItem> _JournalItemRepository) : IQueryHandler<GetCustomerBalanceQuery, decimal>
    {
        public async Task<Result<decimal>> Handle(GetCustomerBalanceQuery request, CancellationToken cancellationToken)
        {
            var (dealer, _, errors) = await _Validator.ValidateCustomerAsync(request.DealerId, cancellationToken);
            if (errors.Count > 0 || dealer?.AccountId is not > 0)
                return new Result<decimal>(HttpStatusCode.BadRequest, 0, errors.Count > 0 ? errors : [new Error("Customer does not have a linked receivable account.")]);

            var accountId = dealer.AccountId.Value;
            var asOfDate = request.AsOfDate?.Date;

            var items = await _JournalItemRepository.GetListByFilterAsync(e =>
                e.AccountId == accountId &&
                e.Journal!.Status != Status.Deleted &&
                e.Journal!.Status != Status.Cancel &&
                (e.Journal!.Posted || (e.Journal!.RefranceTable != null && e.Journal!.RefranceTable != "")) &&
                (asOfDate == null || e.Journal!.Date.Date <= asOfDate));

            var balance = (items ?? []).Sum(e => e.Debit - e.Credit);
            return new Result<decimal>(HttpStatusCode.OK, balance, null);
        }
    }
}
