namespace Receivables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// A customer's outstanding balance computed from the AR subledger itself — Sum(OutstandingAmount)
/// over that customer's Open/PartiallySettled Receivables — as opposed to GetCustomerBalanceQuery,
/// which is derived live from GL Journal history. See GetBalanceReconciliationQuery to compare the
/// two (brief §14/§51's control-account reconciliation check).
/// </summary>
public record GetCustomerSubledgerBalanceQuery(long CustomerId) : IQuery<decimal>;
