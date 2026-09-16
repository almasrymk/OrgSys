namespace Advances.Application.Custodies.Queries;

public sealed record CustodyHandoverDto(
    long Id,
    long FromHolderId,
    long ToHolderId,
    DateTime TransferDate,
    decimal TransferredAmount,
    string? Reason,
    long ApprovedByUserId);

public sealed record CustodyDto(
    long Id,
    string? Code,
    long HolderId,
    string Purpose,
    DateTime? DueDate,
    long CurrencyId,
    decimal Rate,
    decimal IssuedAmount,
    decimal SettledAmount,
    decimal ReturnedAmount,
    decimal OutstandingAmount,
    CustodyStatus LifecycleStatus,
    DateTime? IssueDate,
    long? IssuingFinancialTransactionId,
    long? ReturnFinancialTransactionId,
    string? Notes,
    DateTime Date,
    long? BranchId,
    List<CustodyHandoverDto> Handovers);
