namespace Inventory.Contracts.Transactions;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for "mirror an owning document's status onto its linked inventory Transaction"
/// (Sales Invoice Cancel/Redo cascades here). Handled by Inventory.Application — see
/// docs/dependency-rules.md.
/// </summary>
public record SetTransactionStatusCommand(long TransactionId, Status Status) : ICommand;
