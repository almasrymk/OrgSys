using Application.Abstraction.Command;
using Application.Commands.Org.Financials.Integration.JournalTransaction;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using System.Net;

namespace Application.Commands.Org.Transactions.Transaction.Commands;

public sealed record RedoTransactionCommand(long Id) : ICommand, IUpdateCommand<Result>;

public sealed class RedoTransactionCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<Domain.Entities.Transaction> repository,
    IMapper mapper,
    IServiceProvider provider)
    : UpdateCommandHandler<RedoTransactionCommand, Domain.Entities.Transaction>(unitOfWork, repository, mapper, provider)
{
    public override async Task<Result> Handle(RedoTransactionCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var transaction = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (transaction == null)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.NotFound, [new Error("Transaction not found")]);
            }

            if (transaction.Status == Domain.Enums.Status.New)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.OK, null);
            }

            transaction.Status = Domain.Enums.Status.New;
            await new TransactionJournalIntegration(provider)
                .SetStatusByTransactionIdAsync(transaction.Id, Domain.Enums.Status.New);

            if (await unitOfWork.SaveChangeAsync(cancellationToken) > 0)
            {
                await unitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }

            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}
