namespace Inventory.Application.Transactions.Commands;

using OrgSys.SharedKernel;
using System.Net;
using Inventory.Application.Transactions.Integration;
using Inventory.Contracts.Transactions;

/// <summary>
/// Handles the Contracts-facing SetTransactionStatusCommand — mirrors an owning document's
/// status (e.g. a Sales Invoice being Cancelled/Redone) onto its linked inventory Transaction and
/// the transaction's own journal. Does not call SaveChanges itself: the caller (e.g. Sales'
/// Invoice Cancel/Redo handler) persists both changes together in one UnitOfWork, exactly as the
/// inline version this replaces did — see docs/dependency-rules.md.
/// </summary>
public sealed class SetTransactionStatusCommandHandler(
    IRepository<Transaction> transactionRepository,
    IServiceProvider provider) : ICommandHandler<SetTransactionStatusCommand>
{
    public async Task<Result> Handle(SetTransactionStatusCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByFilterAsync(e => e.Id == request.TransactionId, string.Empty);
        if (transaction == null)
            return new Result(HttpStatusCode.InternalServerError, [new Error("Transaction not found")]);

        transaction.Status = request.Status;
        await new TransactionJournalPostingService(provider).SetStatusByTransactionIdAsync(transaction.Id, request.Status);

        return new Result(HttpStatusCode.OK, null);
    }
}
