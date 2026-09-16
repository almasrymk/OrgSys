namespace MasterData.Contracts.Lookups;

using OrgSys.SharedKernel;

public record GetCountryNamesQuery(IReadOnlyCollection<long> CountryIds) : IQuery<Dictionary<long, string?>>;
