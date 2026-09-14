using MediatR;
using Microsoft.AspNetCore.Mvc;
using Receivables.Contracts.Balances;
using Receivables.Contracts.Receivables;

namespace API.Controllers.Org.Financials;

/// <summary>Accounts Receivable (Customer) balance and aging — read-side. GetBalance/GetAging are
/// computed live from Journal/JournalItem history (the long-standing GL-derived view — mirrors
/// <see cref="PayableController"/> for the AR side). The Outstanding/Overdue/SubledgerBalance/
/// SubledgerAging/Reconciliation endpoints are backed by the Receivables open-item subledger added
/// in docs/architecture/receivables-ddd-migration.md §12 Phase 7 — only reflects Receivables created
/// since that subledger started recording them (Phase 4/5 onward), see Reconciliation's own remarks.
/// Opening balance posting lives on <c>FinancialController</c>
/// ("Financial/Receivable/OpeningBalance") alongside the unified Financial/Journal Receipt path —
/// not duplicated here.</summary>
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

    [HttpGet("Outstanding")]
    public async Task<Result<List<ReceivableDto>>> GetOutstanding(long? dealerId, CancellationToken cancellationToken) =>
        await sender.Send(new GetOutstandingReceivablesQuery(dealerId), cancellationToken);

    [HttpGet("Overdue")]
    public async Task<Result<List<ReceivableDto>>> GetOverdue(long? dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetOverdueReceivablesQuery(dealerId, asOfDate), cancellationToken);

    [HttpGet("{dealerId:long}/SubledgerBalance")]
    public async Task<Result<decimal>> GetSubledgerBalance(long dealerId, CancellationToken cancellationToken) =>
        await sender.Send(new GetCustomerSubledgerBalanceQuery(dealerId), cancellationToken);

    [HttpGet("{dealerId:long}/SubledgerAging")]
    public async Task<Result<AgingBucketDto>> GetSubledgerAging(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetSubledgerAgingQuery(dealerId, asOfDate), cancellationToken);

    [HttpGet("{dealerId:long}/BalanceReconciliation")]
    public async Task<Result<BalanceReconciliationDto>> GetBalanceReconciliation(long dealerId, DateTime? asOfDate, CancellationToken cancellationToken) =>
        await sender.Send(new GetBalanceReconciliationQuery(dealerId, asOfDate), cancellationToken);
}
