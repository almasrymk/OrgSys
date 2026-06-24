namespace Application.Commands.Org.Transactions.Inventory.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateInventoryCommand : Application.DTOs.InventoryDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Inventory> _Repository , IMapper mapper) : CreateCommandHandler<CreateInventoryCommand, Domain.Entities.Inventory>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}