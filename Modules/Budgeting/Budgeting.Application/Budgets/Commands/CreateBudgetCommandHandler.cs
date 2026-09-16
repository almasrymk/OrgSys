namespace Budgeting.Application.Budgets.Commands;

using Budgeting.Contracts.Budgets;
using Budgeting.Domain.Exceptions;
using System.Net;

public sealed class CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IRepository<Budget> repository)
    : ICommandHandler<CreateBudgetCommand, long>
{
    public async Task<Result<long>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var budget = Budget.Create(request.Name, request.FiscalYearId, request.PeriodStart, request.PeriodEnd, request.DepartmentId);
            foreach (var line in request.Lines ?? [])
                budget.AddLine(line.AccountId, line.Amount);

            await repository.CreateAsync(budget);
            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error("Error")]);

            return new Result<long>(HttpStatusCode.OK, budget.Id, null);
        }
        catch (BudgetDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
