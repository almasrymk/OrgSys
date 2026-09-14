namespace Receivables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// Compares the GL-derived balance (GetCustomerBalanceQuery) against the subledger-derived balance
/// (GetCustomerSubledgerBalanceQuery) for one customer — the runnable version of brief §14/§51's
/// "AR Control Account GL Balance == AR Subledger Balance" acceptance check. A non-zero Difference
/// is expected, not necessarily a bug, for any customer with AR history predating this subledger's
/// Phase 4/5 rollout (no historical backfill was done — see the migration doc) or whose payments
/// have not yet been wired (Phase 6 only covers PostFinancialTransactionCommand/ReferenceType.Customer
/// receipts, not every legacy Treasury payment path — see the migration doc's Phase 6 analysis).
/// </summary>
public record GetBalanceReconciliationQuery(long CustomerId, DateTime? AsOfDate) : IQuery<BalanceReconciliationDto>;

public record BalanceReconciliationDto(decimal GlBalance, decimal SubledgerBalance, decimal Difference);
