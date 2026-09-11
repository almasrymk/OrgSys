namespace Inventory.Contracts.Transactions;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for "delete the inventory Transaction linked to a Sales Invoice being
/// deleted". Deliberately distinct from Inventory's own (Contracts-facing) standalone
/// DeleteTransactionCommand, which refuses to delete a transaction that has a source invoice —
/// this command IS the invoice-initiated cascade path. Handled by Inventory.Application.
/// </summary>
public record DeleteTransactionByInvoiceCommand(long TransactionId) : ICommand;
