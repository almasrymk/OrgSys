namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateInventoryCommand : Entity.ModelView.InventoryModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Inventory> _Repository , IMapper mapper) : CreateCommandHandler<CreateInventoryCommand, Entity.Model.Inventory>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}