namespace Inventory.Application.Transactions.Commands;

using OrgSys.SharedKernel;
using global::Application.Commands.Org.Financials.Integration.JournalTransaction;
using OrgSys.SharedKernel;
using System.Net;

public sealed record CreateJournalByTransactionCommand(long TransactionId) : ICommand;

public sealed class CreateJournalByTransactionCommandHandler(
    IRepository<Inventory.Domain.Transaction> repository,
    IUnitOfWork unitOfWork,
    IServiceProvider provider) : ICommandHandler<CreateJournalByTransactionCommand>
{
    public async Task<Result> Handle(CreateJournalByTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByFilterAsync(e => e.Id == request.TransactionId, string.Empty);
        if (transaction == null)
            return new Result(HttpStatusCode.NotFound, [new Error("Transaction not found")]);
        if (transaction.TypeId is < 1 or > 6)
            return new Result(HttpStatusCode.BadRequest, [new Error("This transaction type does not support journal creation")]);

        try
        {
            await new TransactionJournalIntegration(provider).SyncAsync(transaction, force: true);
            await repository.UpdateAsync(transaction);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}
