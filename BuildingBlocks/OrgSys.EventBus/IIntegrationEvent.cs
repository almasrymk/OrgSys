using MediatR;

namespace OrgSys.SharedKernel;

/// <summary>
/// A cross-module integration event (e.g. SalesInvoicePosted, JournalEntryPosted).
/// Implemented as a MediatR notification for now — see
/// docs/modular-monolith-target-architecture.md §5 for why: publishing/handling stays inside
/// the same request and DB transaction as the triggering command until module-specific
/// DbContexts make eventual consistency necessary. Swapping the transport (RabbitMQ, Kafka,
/// Azure Service Bus) later only requires changing the publisher, not every handler.
/// </summary>
public interface IIntegrationEvent : INotification
{
    Guid EventId { get; }

    DateTime OccurredOn { get; }
}

/// <summary>Convenience base implementing <see cref="OccurredOn"/> and <see cref="EventId"/>.</summary>
public abstract record IntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
