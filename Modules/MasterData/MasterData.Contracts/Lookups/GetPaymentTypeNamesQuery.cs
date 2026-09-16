namespace MasterData.Contracts.Lookups;

using OrgSys.SharedKernel;

public record GetPaymentTypeNamesQuery(IReadOnlyCollection<long> PaymentTypeIds) : IQuery<Dictionary<long, string?>>;
