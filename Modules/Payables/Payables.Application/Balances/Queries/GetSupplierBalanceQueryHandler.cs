namespace Payables.Application.Balances.Queries
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using MediatR;
    using OrgSys.SharedKernel;
    using Payables.Contracts.Balances;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetSupplierBalanceQuery — Payables' own "Outstanding Balance"
    /// view of a supplier, computed the same way as Parties' generic GetDealerBalanceQuery
    /// (Sum(Debit-Credit) of valid Journal history against the supplier's payable account) but
    /// resolved/validated through IPayableAccountValidator so this module owns the AP-specific
    /// meaning of "balance" per its own Contracts surface. Ledger activity itself is read through
    /// Accounting.Contracts.Postings.GetAccountActivityQuery instead of an IRepository&lt;JournalItem&gt;
    /// reference across the module boundary.
    /// </summary>
    public sealed class GetSupplierBalanceQueryHandler(
        IPayableAccountValidator _Validator,
        ISender sender) : IQueryHandler<GetSupplierBalanceQuery, decimal>
    {
        public async Task<Result<decimal>> Handle(GetSupplierBalanceQuery request, CancellationToken cancellationToken)
        {
            var (dealer, _, errors) = await _Validator.ValidateSupplierAsync(request.DealerId, cancellationToken);
            if (errors.Count > 0 || dealer?.AccountId is not > 0)
                return new Result<decimal>(HttpStatusCode.BadRequest, 0, errors.Count > 0 ? errors : [new Error("Supplier does not have a linked payable account.")]);

            var items = (await sender.Send(new GetAccountActivityQuery(dealer.AccountId.Value, request.AsOfDate), cancellationToken)).Response ?? [];

            // Payable balances are naturally stored as Credit (what we owe); flip the sign so a
            // positive result reads as "amount owed to this supplier", matching AR's convention.
            var balance = items.Sum(e => e.Credit - e.Debit);
            return new Result<decimal>(HttpStatusCode.OK, balance, null);
        }
    }
}
