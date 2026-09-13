namespace Accounting.Contracts.Accounts;

/// <summary>
/// Public read shape for an Account, used by other modules instead of an EF navigation or direct
/// Accounting.Domain.Account reference. Mirrors Parties.Contracts.Dealers.DealerLookupDto.
/// </summary>
public sealed record AccountLookupDto(long Id, string? Name, string? Code, long AccountTypeId, string? AccountTypeName, bool IsPostable, bool IsActive);
