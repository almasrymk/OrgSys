namespace MasterData.Contracts.Lookups;

using OrgSys.SharedKernel;

public record GetCityNamesQuery(IReadOnlyCollection<long> CityIds) : IQuery<Dictionary<long, string?>>;
