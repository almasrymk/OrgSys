namespace Application.Commands.Org.Setting.Stock.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;

    public sealed record UpdateStockCommand(long Id , string Name, long BranchId) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Stock> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateStockCommand, Domain.Entities.Stock>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}