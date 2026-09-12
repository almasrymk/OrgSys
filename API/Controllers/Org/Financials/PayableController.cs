using Payables.Application.OpeningBalance.Commands;
using Payables.Contracts.Balances;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials;

/// <summary>Accounts Payable (Supplier) opening balance, balance, and aging — the balance/aging
/// are read-side, computed live from Journal/JournalItem history (mirrors
/// <see cref="ReceivableController"/> for the AR side). Supplier Payment posting/reversal/listing
/// lives on the unified <c>Financial</c>/<c>Journal</c> Receipt/Payment path instead of a dedicated
/// AP screen — see FinancialAccountArchitecture review.</summary>
[Route("[controller]")]
[ApiController]
public sealed class PayableController(ISender sender) : ControllerBase
{
    [HttpPost("OpeningBalance")]
    public Task<Result> SetSupplierOpeningBalance(
        [FromBody] SetSupplierOpeningBalanceCommand command,
        CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("{dealerId:long}/Balance")]
    public async Task<Result<decimal>> GetBalance(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetSupplierBalanceQuery(dealerId, asOfDate), cancellationToken);

    [HttpGet("{dealerId:long}/Aging")]
    public async Task<Result<AgingBucketDto>> GetAging(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetSupplierAgingQuery(dealerId, asOfDate), cancellationToken);
}
