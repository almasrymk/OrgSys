namespace MasterData.Contracts.Currencies;

using OrgSys.SharedKernel;

public record GetCurrencyLookupsQuery(IReadOnlyCollection<long> CurrencyIds)
    : IQuery<Dictionary<long, CurrencyLookupDto>>;
