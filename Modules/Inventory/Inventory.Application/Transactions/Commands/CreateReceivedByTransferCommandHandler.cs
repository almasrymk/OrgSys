namespace Inventory.Application.Transactions.Commands;

using Inventory.Application.Transactions.Integration;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using System.Net;

public sealed record CreateReceivedByTransferCommand(long TransferId) : ICommand;

public sealed class CreateReceivedByTransferCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<Inventory.Domain.Transaction> repository,
    IServiceProvider provider) : ICommandHandler<CreateReceivedByTransferCommand>
{
    public async Task<Result> Handle(CreateReceivedByTransferCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var transfer = await repository.GetByFilterAsync(
                e => e.Id == request.TransferId && e.TypeId == 3,
                "TransactionProducts");
            if (transfer == null)
            {
                await unitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.NotFound, [new Error("Transfer not found")]);
            }

            await new TransferReceivedIntegration(provider).SyncAsync(transfer, cancellationToken, force: true);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            await unitOfWork.CommitAsync();
            return new Result(HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}
