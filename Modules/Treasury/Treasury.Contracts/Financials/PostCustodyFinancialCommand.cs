namespace Treasury.Contracts.Financials;

using OrgSys.SharedKernel;

/// <summary>
/// Posts a Treasury Financial for an Advances custody movement and returns the new Financial.Id.
/// Disbursement (true) is a Payment out of the financial account; return (false) is a Receipt in.
/// Advances never writes Financial itself — it sends this command then stamps the id on Custody.
/// </summary>
public record PostCustodyFinancialCommand(
    bool Disbursement,
    long FinancialAccountId,
    long CounterAccountId,
    decimal Amount,
    long CurrencyId,
    decimal ExchangeRate,
    DateTime TransactionDate,
    long HolderId,
    long CustodyId,
    string? Description,
    long CreateUserId,
    long? BranchId,
    long? ShiftId) : ICommand<long>;
