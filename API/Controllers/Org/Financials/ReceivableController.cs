using MediatR;
using Microsoft.AspNetCore.Mvc;
using Receivables.Contracts.Balances;

namespace API.Controllers.Org.Financials;

/// <summary>Accounts Receivable (Customer) balance and aging — read-side, computed live from
/// Journal/JournalItem history. Mirrors <see cref="PayableController"/> for the AR side. Opening
/// balance posting lives on <c>FinancialController</c> ("Financial/Receivable/OpeningBalance")
/// alongside the unified Financial/Journal Receipt path — not duplicated here.</summary>
[Route("[controller]")]
[ApiController]
public sealed class ReceivableController(ISender sender) : ControllerBase
{
    [HttpGet("{dealerId:long}/Balance")]
    public async Task<Result<decimal>> GetBalance(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetCustomerBalanceQuery(dealerId, asOfDate), cancellationToken);

    [HttpGet("{dealerId:long}/Aging")]
    public async Task<Result<AgingBucketDto>> GetAging(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetCustomerAgingQuery(dealerId, asOfDate), cancellationToken);
}
