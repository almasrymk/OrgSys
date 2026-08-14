namespace Domain.Enums;

public enum FinancialAccountType
{
    CashBox = 1,
    Bank = 2
}

public enum FinancialTransactionDirection
{
    In = 1,
    Out = 2
}

public enum FinancialReferenceType
{
    Other = 0,
    Customer = 1,
    Supplier = 2,
    Employee = 3,
    Expense = 4,
    Income = 5,
    Invoice = 6,
    Payment = 7,
    Loan = 8,
    Cheque = 9,
    PaymentGateway = 10,
    Transfer = 11
}
