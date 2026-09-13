namespace Accounting.Contracts.Accounts;

using OrgSys.SharedKernel;

/// <summary>Public contract for resolving a single Account by id (e.g. Treasury's GL counter-account validation). Handled by Accounting.Application. Returns null when not found.</summary>
public sealed record GetAccountQuery(long AccountId) : IQuery<AccountLookupDto?>;
