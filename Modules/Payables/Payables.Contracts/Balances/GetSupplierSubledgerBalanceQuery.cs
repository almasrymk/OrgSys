namespace Payables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// A supplier's outstanding balance computed from the AP subledger itself — Sum(OutstandingAmount)
/// over that supplier's Open/PartiallySettled Payables — as opposed to GetSupplierBalanceQuery,
/// which is derived live from GL Journal history. See GetBalanceReconciliationQuery to compare the
/// two.
/// </summary>
public record GetSupplierSubledgerBalanceQuery(long SupplierId) : IQuery<decimal>;
