using Payables.Application.OpeningBalance.Commands;
using Payables.Contracts.Balances;
using Payables.Contracts.Payables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials;

/// <summary>Accounts Payable (Supplier) opening balance, balance, and aging. GetBalance/GetAging are
/// computed live from Journal/JournalItem history (the long-standing GL-derived view — mirrors
/// <see cref="ReceivableController"/> for the AR side). The Outstanding/Overdue/SubledgerBalance/
/// SubledgerAging/Reconciliation endpoints are backed by the Payables open-item subledger — only
/// reflects Payables created since that subledger started recording them, see Reconciliation's own
/// remarks. Supplier Payment posting/reversal/listing lives on the unified
/// <c>Financial</c>/<c>Journal</c> Receipt/Payment path instead of a dedicated AP screen — see
/// FinancialAccountArchitecture review.</summary>
[Authorize]
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

    [HttpGet("Outstanding")]
    public async Task<Result<List<PayableDto>>> GetOutstanding(long? dealerId, CancellationToken cancellationToken) =>
        await sender.Send(new GetOutstandingPayablesQuery(dealerId), cancellationToken);

    [HttpGet("Overdue")]
    public async Task<Result<List<PayableDto>>> GetOverdue(long? dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetOverduePayablesQuery(dealerId, asOfDate), cancellationToken);

    [HttpGet("{dealerId:long}/SubledgerBalance")]
    public async Task<Result<decimal>> GetSubledgerBalance(long dealerId, CancellationToken cancellationToken) =>
        await sender.Send(new GetSupplierSubledgerBalanceQuery(dealerId), cancellationToken);

    [HttpGet("{dealerId:long}/SubledgerAging")]
    public async Task<Result<AgingBucketDto>> GetSubledgerAging(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetSubledgerAgingQuery(dealerId, asOfDate), cancellationToken);

    [HttpGet("{dealerId:long}/BalanceReconciliation")]
    public async Task<Result<BalanceReconciliationDto>> GetBalanceReconciliation(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetBalanceReconciliationQuery(dealerId, asOfDate), cancellationToken);
}
