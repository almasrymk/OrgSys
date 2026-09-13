namespace Accounting.Contracts.Accounts;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Account display names by id (e.g. a Stock/FinancialAccount
/// list screen showing AccountName without an EF Include across the module boundary — the
/// GeneralLedger migration removed those Domain-to-Domain navigations). Handled by
/// Accounting.Application. Missing ids are simply absent from the result. Mirrors
/// Parties.Contracts.Dealers.GetDealerNamesQuery / Inventory's GetProductNamesQuery exactly.
/// </summary>
public record GetAccountNamesQuery(IReadOnlyCollection<long> AccountIds) : IQuery<Dictionary<long, string?>>;
