namespace Advances.Application;

/// <summary>
/// Non-functional type used only as a reflection anchor for this assembly (MediatR/FluentValidation
/// registration in ServiceCollectionExtensions.AddAdvancesModule, and Architecture.Tests) — no
/// commands/queries exist yet. Mirrors Advances.Domain.AssemblyMarker; delete once a real
/// command/handler gives this assembly a natural anchor type, the same way
/// Payables.Application.OpeningBalance.Commands.SetSupplierOpeningBalanceCommand does for Payables.
/// </summary>
public sealed class AssemblyMarker
{
    private AssemblyMarker() { }
}
