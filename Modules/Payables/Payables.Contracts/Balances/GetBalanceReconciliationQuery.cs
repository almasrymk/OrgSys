namespace Payables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// Compares the GL-derived balance (GetSupplierBalanceQuery) against the subledger-derived balance
/// (GetSupplierSubledgerBalanceQuery) for one supplier. A non-zero Difference is expected, not
/// necessarily a bug, for any supplier with AP history predating this subledger's rollout (no
/// historical backfill was done) or whose payments have not yet been wired (only
/// PostFinancialTransactionCommand/ReferenceType.Supplier, Direction.Out payments are covered).
/// </summary>
public record GetBalanceReconciliationQuery(long SupplierId, DateTime? AsOfDate) : IQuery<BalanceReconciliationDto>;

public record BalanceReconciliationDto(decimal GlBalance, decimal SubledgerBalance, decimal Difference);
