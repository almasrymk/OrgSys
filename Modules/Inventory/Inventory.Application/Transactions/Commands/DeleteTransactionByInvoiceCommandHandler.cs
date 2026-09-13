namespace Inventory.Application.Transactions.Commands;

using OrgSys.SharedKernel;
using System.Net;
using Inventory.Application.Transactions.Integration;
using Inventory.Contracts.Transactions;

/// <summary>
/// Handles the Contracts-facing DeleteTransactionByInvoiceCommand — the invoice-initiated
/// cascade path, deliberately without the "created from invoice" guard the standalone
/// DeleteTransactionCommand enforces (that guard exists precisely to stop direct/standalone
/// deletion; this command IS the invoice's own delete flow). Mirrors the logic Sales' Invoice
/// DeleteCommandHandler/DeleteListCommandHandler used to run inline against Inventory.Domain
/// directly — see docs/dependency-rules.md.
/// </summary>
public sealed class DeleteTransactionByInvoiceCommandHandler(
    IRepository<Transaction> transactionRepository,
    IServiceProvider provider) : ICommandHandler<DeleteTransactionByInvoiceCommand>
{
    public async Task<Result> Handle(DeleteTransactionByInvoiceCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByFilterAsync(e => e.Id == request.TransactionId, "TransactionProducts");
        if (transaction == null)
            return new Result(HttpStatusCode.OK, null);

        await new TransactionJournalPostingService(provider).DeleteByTransactionIdAsync(transaction.Id);
        transaction.TransactionProducts?.Clear();
        await transactionRepository.ShiftDeleteAsync(t => t.Id == transaction.Id);

        return new Result(HttpStatusCode.OK, null);
    }
}
