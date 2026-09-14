namespace Receivables.Contracts.Receivables;

/// <summary>Read-only projection of one AR open item, backed by the Receivables.Domain.Receivable open-item table (not the GL) — see docs/architecture/receivables-ddd-migration.md §12 Phase 7.</summary>
public record ReceivableDto(
    long Id,
    long CustomerId,
    string SourceDocumentType,
    long SourceDocumentId,
    string? SourceDocumentNumber,
    DateTime DocumentDate,
    DateTime DueDate,
    long CurrencyId,
    decimal OriginalAmount,
    decimal OutstandingAmount,
    string LifecycleStatus,
    bool IsOverdue);
