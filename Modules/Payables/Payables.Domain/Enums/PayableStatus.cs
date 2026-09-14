namespace Payables.Domain;

/// <summary>
/// Lifecycle of one AP open item. "Overdue" is deliberately not a stored state here — it is derived
/// (OutstandingAmount > 0 &amp;&amp; DueDate before the business date, see Payable.IsOverdue) rather
/// than persisted, since it changes purely with the passage of time and not because of any action
/// taken on the payable. Mirrors Receivables.Domain.ReceivableStatus.
/// </summary>
public enum PayableStatus
{
    Open = 0,
    PartiallySettled = 10,
    Settled = 20,
    Cancelled = 30,
    WrittenOff = 40
}
