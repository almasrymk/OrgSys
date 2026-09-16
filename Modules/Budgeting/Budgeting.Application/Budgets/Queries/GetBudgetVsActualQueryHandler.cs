namespace Budgeting.Application.Budgets.Queries;

using Accounting.Contracts.Postings;
using Budgeting.Contracts.Budgets;
using MediatR;
using System.Net;

public sealed class GetBudgetVsActualQueryHandler(IRepository<Budget> repository, ISender sender)
    : IQueryHandler<GetBudgetVsActualQuery, BudgetVsActualDto>
{
    public async Task<Result<BudgetVsActualDto>> Handle(GetBudgetVsActualQuery request, CancellationToken cancellationToken)
    {
        var budget = await repository.GetByFilterAsync(e => e.Id == request.BudgetId, "Lines");
        if (budget is null || budget.Id == 0)
            return new Result<BudgetVsActualDto>(HttpStatusCode.NotFound, null, [new Error("Budget not found.")]);

        var lines = new List<BudgetVsActualLineDto>();
        foreach (var line in budget.Lines)
        {
            var activity = await sender.Send(new GetAccountActivityQuery(line.AccountId, budget.PeriodEnd), cancellationToken);
            var actual = (activity.Response ?? [])
                .Where(a => a.Date >= budget.PeriodStart && a.Date <= budget.PeriodEnd)
                .Sum(a => a.Debit - a.Credit);
            lines.Add(new BudgetVsActualLineDto(line.AccountId, line.Amount, actual, line.Amount - actual));
        }

        return new Result<BudgetVsActualDto>(
            HttpStatusCode.OK,
            new BudgetVsActualDto(budget.Id, budget.Name, budget.FiscalYearId, budget.DepartmentId, lines),
            null);
    }
}
