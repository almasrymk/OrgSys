using Budgeting.Contracts.Budgets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Budgeting;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BudgetController(ISender sender) : ControllerBase
{
    [HttpPost("Create")]
    public Task<Result<long>> Create([FromBody] CreateBudgetCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("VsActual")]
    public Task<Result<BudgetVsActualDto>> VsActual(long budgetId, CancellationToken cancellationToken) =>
        sender.Send(new GetBudgetVsActualQuery(budgetId), cancellationToken);
}
