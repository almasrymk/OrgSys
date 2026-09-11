namespace Inventory.Contracts.Transactions;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for "create/sync the inventory Transaction linked to a Sales Invoice".
/// Sales.Application sends this via MediatR without referencing Inventory.Domain/Application —
/// see docs/dependency-rules.md. Handled by Inventory.Application.
/// </summary>
public record CreateTransactionByInvoiceCommand(long Id, bool RespectAutoCreatePreference = false)
    : ICommand, ICreateCommand<Result>;
