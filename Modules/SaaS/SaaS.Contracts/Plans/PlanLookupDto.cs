namespace SaaS.Contracts.Plans;

public record PlanLookupDto(long Id, string Name, int? MaxUsers, int? MaxCompanies, int? MaxBranches, int? MaxWarehouses, int? MaxTransactionsPerMonth);
