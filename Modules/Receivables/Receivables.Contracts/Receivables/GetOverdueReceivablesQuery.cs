namespace Receivables.Contracts.Receivables;

using OrgSys.SharedKernel;

/// <summary>Open items past their DueDate as of AsOfDate (defaults to today) — optionally for one customer. Overdue is derived (Receivable.IsOverdue), not a stored lifecycle state — see ReceivableStatus's own doc comment.</summary>
public record GetOverdueReceivablesQuery(long? CustomerId, DateTime? AsOfDate) : IQuery<List<ReceivableDto>>;
