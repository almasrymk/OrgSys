namespace Inventory.Domain.Enums;

/// <summary>Minimum status set needed for brief §17's invariants (a serial cannot be issued twice,
/// cannot exist in two locations, must stay traceable through transfer/return).</summary>
public enum SerialStatus
{
    Available = 0,
    Reserved = 10,
    Issued = 20,
    Returned = 30,
    Damaged = 40,
    Lost = 50,
    Quarantine = 60
}
