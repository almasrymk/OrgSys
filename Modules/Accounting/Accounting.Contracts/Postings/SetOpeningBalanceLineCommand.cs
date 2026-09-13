namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Upserts a party's own line inside the single shared per-fiscal-year Opening Balance journal
/// (Receivables/Payables Customer/Supplier opening balances — see the Accounting DDD cleanup
/// report) and rebalances a clearing line, exactly as Journal.SetOpeningBalanceLine does. Finds
/// or creates the fiscal year's Opening Balance journal (JournalType.IsOpeningBlance) the same way
/// the manual Journal workflow already does; the caller supplies only the line to set and the
/// clearing account to rebalance against. Once that journal is Posted (like every other Journal)
/// it is immutable — this command then refuses further edits.
/// </summary>
public sealed record SetOpeningBalanceLineCommand(
    long FiscalYearId,
    long AccountId,
    decimal Debit,
    decimal Credit,
    string? Note,
    long ClearingAccountId,
    long CreateUserId) : ICommand;
