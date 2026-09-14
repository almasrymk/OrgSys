namespace Receivables.Domain;

/// <summary>
/// Lifecycle of one AR open item. "Overdue" is deliberately not a stored state here — it is derived
/// (OutstandingAmount > 0 &amp;&amp; DueDate before the business date, see Receivable.IsOverdue) rather
/// than persisted, since it changes purely with the passage of time and not because of any action
/// taken on the receivable.
/// </summary>
public enum ReceivableStatus
{
    Open = 0,
    PartiallySettled = 10,
    Settled = 20,
    Cancelled = 30,
    WrittenOff = 40
}
