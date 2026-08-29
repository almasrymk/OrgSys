using Domain.Enums;

namespace Application.DTOs;

public class FinancialAccountDto : Domain.Entities.BaseModel
{
    public string Name { get; set; } = string.Empty;
    public FinancialAccountType FinancialAccountType { get; set; }
    public long? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? AccountCode { get; set; }
    public long? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public bool IsActive { get; set; } = true;
    public long? BranchId { get; set; }
    public long? KeeperUserId { get; set; }
    public long? BankId { get; set; }
    public string? BankName { get; set; }
    public long? BankBranchId { get; set; }
    public string? BankBranchDisplayName { get; set; }
    public string? AccountNumber { get; set; }
    public string? IBAN { get; set; }
    public string? SwiftCode { get; set; }
}

public sealed class PostFinancialTransactionDto
{
    public long FinancialAccountId { get; set; }
    public FinancialTransactionType FinancialTypeId { get; set; }
    public FinancialTransactionDirection Direction { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; } = 1;
    public DateTime TransactionDate { get; set; }
    public FinancialReferenceType ReferenceType { get; set; }
    public long? ReferenceId { get; set; }
    public string? ReferenceNumber { get; set; }
    public long CounterAccountId { get; set; }
    public long? DealerId { get; set; }
    public string? Description { get; set; }
    public long CreateUserId { get; set; }
    public long? BranchId { get; set; }
    public long? ShiftId { get; set; }
}
