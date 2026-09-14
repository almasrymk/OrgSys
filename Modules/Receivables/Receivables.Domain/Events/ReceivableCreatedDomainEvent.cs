namespace Receivables.Domain.Events;

/// <summary>
/// Raised when a Receivable is created. Not collected inside Receivable.Create — the Id does not
/// exist yet at that point (EF-generated on SaveChanges), the same reason
/// Accounting.Domain.Journal raises JournalReversedDomainEvent from the Application handler after
/// the save succeeds rather than inside CreateReversal. The Application-layer command handler that
/// persists a new Receivable raises this afterward, once Id is known.
/// </summary>
public sealed record ReceivableCreatedDomainEvent(long ReceivableId, long CustomerId, SourceDocumentType SourceDocumentType, long SourceDocumentId, decimal OriginalAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
