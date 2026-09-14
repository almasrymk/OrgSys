namespace Payables.Contracts.Payables;

using OrgSys.SharedKernel;

/// <summary>Open items past their DueDate as of AsOfDate (defaults to today) — optionally for one supplier. Overdue is derived (Payable.IsOverdue), not a stored lifecycle state.</summary>
public record GetOverduePayablesQuery(long? SupplierId, DateTime? AsOfDate) : IQuery<List<PayableDto>>;
