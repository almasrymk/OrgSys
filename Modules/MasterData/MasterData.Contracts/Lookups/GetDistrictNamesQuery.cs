namespace MasterData.Contracts.Lookups;

using OrgSys.SharedKernel;

public record GetDistrictNamesQuery(IReadOnlyCollection<long> DistrictIds) : IQuery<Dictionary<long, string?>>;
