namespace Treasury.Application;

public sealed class FinancialTransferDto
{
    public long Id { get; set; }
    public long FromFinancialAccountId { get; set; }
    public long ToFinancialAccountId { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; } = 1;
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public long CreateUserId { get; set; }
    public long? BranchId { get; set; }
    public long? ShiftId { get; set; }
    public string? FromFinancialAccountName { get; set; }
    public string? ToFinancialAccountName { get; set; }
    public OrgSys.SharedKernel.Status Status { get; set; }
    public long? JournalId { get; set; }
}
