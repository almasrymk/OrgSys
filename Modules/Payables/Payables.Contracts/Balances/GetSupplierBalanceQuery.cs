namespace Payables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for a supplier's outstanding payable balance as of an optional date —
/// Sum(Debit - Credit) of valid Journal history posted to the supplier's payable account.
/// Handled by Payables.Application.
/// </summary>
public record GetSupplierBalanceQuery(long DealerId, DateTime? AsOfDate) : IQuery<decimal>;
