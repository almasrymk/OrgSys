namespace Advances.Contracts;

/// <summary>
/// Non-functional type used only as a reflection anchor for this assembly. Empty for now — no other
/// module needs to reach into Advances yet (brief §22/§72's Contracts pattern, mirrored from
/// Treasury.Contracts/Payables.Contracts). Populated in a later phase with whatever
/// IntegrationEvents/queries Reporting or others end up needing (brief §49 statements/reports).
/// </summary>
public sealed class AssemblyMarker
{
    private AssemblyMarker() { }
}
