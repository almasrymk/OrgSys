namespace MasterData.Contracts.Currencies;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Currency display names by id (e.g. a Journal list/search
/// screen showing CurrencyName without an EF Include across the module boundary). Handled by
/// MasterData.Application. Missing ids are simply absent from the result. Mirrors
/// Parties.Contracts.Dealers.GetDealerNamesQuery / Accounting.Contracts.Accounts.GetAccountNamesQuery.
/// </summary>
public record GetCurrencyNamesQuery(IReadOnlyCollection<long> CurrencyIds) : IQuery<Dictionary<long, string?>>;
