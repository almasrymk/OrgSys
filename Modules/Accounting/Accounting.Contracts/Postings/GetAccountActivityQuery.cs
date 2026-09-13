namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Read-only Journal/JournalItem activity for one Account — the same "valid ledger line" filter
/// Post/Reverse/Cancel enforce (Journal not Deleted/Cancelled, Posted or resource-controlled, on
/// or before AsOfDate when given), used by Receivables/Payables balance and aging reports instead
/// of an IRepository&lt;JournalItem&gt; reference across the module boundary (see the Accounting
/// DDD cleanup report). This is a reporting/read-model concern — CQRS does not require read models
/// to go through the aggregate, only the write side (see docs/dependency-rules.md), but the
/// underlying Journal/JournalItem types themselves must still never leak outside Accounting, hence
/// this DTO-only contract.
/// </summary>
public sealed record GetAccountActivityQuery(long AccountId, DateTime? AsOfDate) : IQuery<List<AccountActivityLineDto>>;

public sealed record AccountActivityLineDto(long JournalItemId, DateTime Date, decimal Debit, decimal Credit);
