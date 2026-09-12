namespace Payables.Application.Balances.Queries
{
    using OrgSys.SharedKernel;
    using Accounting.Application;
    using Payables.Contracts.Balances;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetSupplierBalanceQuery — Payables' own "Outstanding Balance"
    /// view of a supplier, computed the same way as Parties' generic GetDealerBalanceQuery
    /// (Sum(Debit-Credit) of valid Journal history against the supplier's payable account) but
    /// resolved/validated through IPayableAccountValidator so this module owns the AP-specific
    /// meaning of "balance" per its own Contracts surface.
    /// </summary>
    public sealed class GetSupplierBalanceQueryHandler(
        IPayableAccountValidator _Validator,
        IRepository<JournalItem> _JournalItemRepository) : IQueryHandler<GetSupplierBalanceQuery, decimal>
    {
        public async Task<Result<decimal>> Handle(GetSupplierBalanceQuery request, CancellationToken cancellationToken)
        {
            var (dealer, _, errors) = await _Validator.ValidateSupplierAsync(request.DealerId, cancellationToken);
            if (errors.Count > 0 || dealer?.AccountId is not > 0)
                return new Result<decimal>(HttpStatusCode.BadRequest, 0, errors.Count > 0 ? errors : [new Error("Supplier does not have a linked payable account.")]);

            var accountId = dealer.AccountId.Value;
            var asOfDate = request.AsOfDate?.Date;

            var items = await _JournalItemRepository.GetListByFilterAsync(e =>
                e.AccountId == accountId &&
                e.Journal!.Status != Status.Deleted &&
                e.Journal!.Status != Status.Cancel &&
                (e.Journal!.Posted || (e.Journal!.RefranceTable != null && e.Journal!.RefranceTable != "")) &&
                (asOfDate == null || e.Journal!.Date.Date <= asOfDate));

            // Payable balances are naturally stored as Credit (what we owe); flip the sign so a
            // positive result reads as "amount owed to this supplier", matching AR's convention.
            var balance = (items ?? []).Sum(e => e.Credit - e.Debit);
            return new Result<decimal>(HttpStatusCode.OK, balance, null);
        }
    }
}
