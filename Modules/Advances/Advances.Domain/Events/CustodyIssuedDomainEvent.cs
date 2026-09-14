namespace Advances.Domain.Events;

/// <summary>
/// Raised by Custody.MarkIssued once the caller (Advances.Application) has confirmed the Treasury
/// payment for this custody was posted — FinancialTransactionId is that Treasury Financial's id,
/// referenced only, never navigated (brief §31/§61: Advances.Domain must not depend on Treasury.Domain).
/// </summary>
public sealed record CustodyIssuedDomainEvent(long CustodyId, long FinancialTransactionId, DateTime IssueDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
