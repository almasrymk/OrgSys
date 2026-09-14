namespace Sales.Domain;

/// <summary>
/// Lifecycle of one SalesOrder. "Delivered" is deliberately the one terminal
/// nothing-left-to-deliver status, reached either by full delivery or by cancelling the remaining
/// undelivered quantity of an already-partially-delivered order (brief §41: cancellation must
/// preserve delivered history, not erase it) — there is no separate "Completed" status duplicating
/// it, matching the same collapsing rule Advances.Domain.CustodyStatus documents for Settled/
/// Returned. SalesOrderCompletedDomainEvent is still raised at that transition regardless of which
/// path reached it (brief §44) — the domain event name and the status name are not required to
/// match. "Cancelled" is reserved for the clean case: the order is cancelled before ANY line has
/// been delivered at all (mirrors Payables.Domain.PayableStatus.Cancelled's "only if untouched" rule).
/// </summary>
public enum SalesOrderStatus
{
    Draft = 0,
    Confirmed = 10,
    PartiallyDelivered = 20,
    Delivered = 30,
    Cancelled = 40
}
