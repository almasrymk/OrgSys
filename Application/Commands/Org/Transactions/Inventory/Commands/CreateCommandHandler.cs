namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using Application.Commands.Org.Transactions.Inventory.Integration;
    using System.Net;

    public sealed class CreateInventoryCommand : Application.DTOs.InventoryDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Inventory> _Repository,
        IMapper mapper, IServiceProvider provider) : CreateCommandHandler<CreateInventoryCommand, Domain.Entities.Inventory>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var inventory = mapper.Map<Domain.Entities.Inventory>(request);
                await _Repository.CreateAsync(inventory);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                await new InventoryAdjustmentIntegration(provider).SyncAsync(inventory, cancellationToken);
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
