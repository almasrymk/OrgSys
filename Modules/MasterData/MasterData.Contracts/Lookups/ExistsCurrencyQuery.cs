namespace MasterData.Contracts.Lookups;

using OrgSys.SharedKernel;

public record ExistsCurrencyQuery(long Id) : IQuery<bool>;
