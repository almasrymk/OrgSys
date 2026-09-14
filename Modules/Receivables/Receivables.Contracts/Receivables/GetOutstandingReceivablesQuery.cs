namespace Receivables.Contracts.Receivables;

using OrgSys.SharedKernel;

/// <summary>Open items (Open/PartiallySettled) — optionally for one customer — ordered oldest DocumentDate first. Excludes Settled/Cancelled/WrittenOff by definition of "outstanding".</summary>
public record GetOutstandingReceivablesQuery(long? CustomerId) : IQuery<List<ReceivableDto>>;
