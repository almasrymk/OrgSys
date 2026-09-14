namespace Payables.Contracts.Payables;

/// <summary>Read-only projection of one AP open item, backed by the Payables.Domain.Payable open-item table (not the GL). Mirrors Receivables.Contracts.Receivables.ReceivableDto.</summary>
public record PayableDto(
    long Id,
    long SupplierId,
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
