namespace Treasury.Domain;

// One-to-one with the seeded Infrastructure/Seed/InitialData.cs FinancialType rows —
// this enum IS the FinancialType table's Id space, not a separate classification.
// Financial.FinancialTypeId is typed as this enum and doubles as the FK to FinancialType.
public enum FinancialTransactionType : long
{
    OpeningBalance = 1,
    Receipt = 2,
    Payment = 3,
    TransferIn = 4,
    Deposit = 5,
    Withdrawal = 6,
    Fee = 7,
    Interest = 8,
    Cheque = 9,
    Adjustment = 10,
    TransferOut = 11
}
