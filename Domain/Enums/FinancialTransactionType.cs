namespace Domain.Enums;

public enum FinancialTransactionType
{
    Receipt = 1,
    Payment = 2,

    TransferOut = 3,
    TransferIn = 4,

    AdvanceIssue = 5,
    AdvanceSettlement = 6,
    AdvanceReturn = 7
}
