namespace Inventory.Application.Transactions.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using Inventory.Application.Transactions.Integration;

    public sealed class CreateTransactionCommand : TransactionDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Transaction> _Repository, IMapper mapper, IServiceProvider provider) : CreateCommandHandler<CreateTransactionCommand, Inventory.Domain.Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var transaction = mapper.Map<Inventory.Domain.Transaction>(request);
                await _Repository.CreateAsync(transaction);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                await new TransactionJournalPostingService(provider).SyncAsync(transaction);
                await new TransferReceivedIntegration(provider).SyncAsync(transaction, cancellationToken);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);
                await _UnitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
