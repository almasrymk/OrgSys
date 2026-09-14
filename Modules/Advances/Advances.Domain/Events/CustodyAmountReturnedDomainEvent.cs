namespace Advances.Domain.Events;

/// <summary>
/// Raised by Custody.Return once the caller has confirmed Treasury received the returned cash —
/// ReturnFinancialTransactionId is that Treasury Financial's id, referenced only (brief §36/§61).
/// </summary>
public sealed record CustodyAmountReturnedDomainEvent(long CustodyId, decimal ReturnedAmount, long ReturnFinancialTransactionId, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
