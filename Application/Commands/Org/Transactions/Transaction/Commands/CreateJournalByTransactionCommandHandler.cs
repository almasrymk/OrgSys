namespace Application.Commands.Org.Transactions.Transaction.Commands;

using Application.Abstraction.Command;
using Application.Commands.Org.Financials.Integration.JournalTransaction;
using Application.Interfaces.CQRS;
using Domain.Abstraction;
using Domain.Shared;
using System.Net;

public sealed record CreateJournalByTransactionCommand(long TransactionId) : ICommand;

public sealed class CreateJournalByTransactionCommandHandler(
    IRepository<Domain.Entities.Transaction> repository,
    IUnitOfWork unitOfWork,
    IServiceProvider provider) : ICommandHandler<CreateJournalByTransactionCommand>
{
    public async Task<Result> Handle(CreateJournalByTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByFilterAsync(e => e.Id == request.TransactionId, string.Empty);
        if (transaction == null)
            return new Result(HttpStatusCode.NotFound, [new Error("Transaction not found")]);
        if (transaction.TypeId is < 1 or > 4)
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
