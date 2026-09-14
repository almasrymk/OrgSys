namespace Payables.Contracts.Payables;

using OrgSys.SharedKernel;

/// <summary>Open items (Open/PartiallySettled) — optionally for one supplier — ordered oldest DocumentDate first. Excludes Settled/Cancelled/WrittenOff by definition of "outstanding".</summary>
public record GetOutstandingPayablesQuery(long? SupplierId) : IQuery<List<PayableDto>>;
