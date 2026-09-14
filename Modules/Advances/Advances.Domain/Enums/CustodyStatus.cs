namespace Advances.Domain;

/// <summary>
/// Lifecycle of one Custody (عهدة). "Returned" is deliberately not a separate terminal status
/// alongside "Settled" — brief §32 lists both "...Settled -> Closed" and "Issued -> Returned/Closed"
/// as valid paths, but a custody whose OutstandingAmount reaches zero purely through Return (no
/// Settle at all) is not conceptually different from one that reaches zero through Settle, or a mix
/// of both — both mean "nothing outstanding," which is exactly what Settled already represents here.
/// Collapsing them avoids two terminal statuses with identical meaning; Close() is the one explicit
/// archival step from Settled either way. Mirrors the same reasoning Payables.Domain.PayableStatus
/// uses for not storing "Overdue" as a state.
/// </summary>
public enum CustodyStatus
{
    Draft = 0,
    Approved = 10,
    Issued = 20,
    PartiallySettled = 30,
    Settled = 40,
    Closed = 50,
    Cancelled = 60
}
