namespace MasterData.Contracts.Lookups;

using OrgSys.SharedKernel;

public record ExistsCountryQuery(long Id) : IQuery<bool>;
