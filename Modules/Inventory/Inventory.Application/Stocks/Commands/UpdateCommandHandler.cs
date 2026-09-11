namespace Inventory.Application.Stocks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateStockCommand(long Id, string Name, long BranchId, long? AccountId) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Stock> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateStockCommand, Inventory.Domain.Stock>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
