namespace Application.Commands.Org.Setting.Stock.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateStockCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Stock> _Repository , IMapper mapper) : CreateCommandHandler<CreateStockCommand, Entity.Model.Stock>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}