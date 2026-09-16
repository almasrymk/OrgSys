namespace Budgeting.Contracts.Budgets;

using OrgSys.SharedKernel;

public sealed record BudgetLineInput(long AccountId, decimal Amount);

public sealed record CreateBudgetCommand(
    string Name,
    long FiscalYearId,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    long? DepartmentId,
    IReadOnlyList<BudgetLineInput> Lines) : ICommand<long>;

public sealed record BudgetVsActualLineDto(long AccountId, decimal BudgetAmount, decimal ActualAmount, decimal Variance);

public sealed record BudgetVsActualDto(
    long BudgetId,
    string? Name,
    long FiscalYearId,
    long? DepartmentId,
    IReadOnlyList<BudgetVsActualLineDto> Lines);

public sealed record GetBudgetVsActualQuery(long BudgetId) : IQuery<BudgetVsActualDto>;
