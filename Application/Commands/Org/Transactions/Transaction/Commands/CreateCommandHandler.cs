namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using Application.Commands.Org.Financials.Integration.JournalTransaction;
    using System.Net;
    using Application.Commands.Org.Transactions.Transaction.Integration;

    public sealed class CreateTransactionCommand : Application.DTOs.TransactionDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Transaction> _Repository, IMapper mapper, IServiceProvider provider) : CreateCommandHandler<CreateTransactionCommand, Domain.Entities.Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var transaction = mapper.Map<Domain.Entities.Transaction>(request);
                await _Repository.CreateAsync(transaction);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                await new TransactionJournalIntegration(provider).SyncAsync(transaction);
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
