namespace Inventory.Application.Stocks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateStockCommand(string Name, long BranchId, long? AccountId) : ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Stock> _Repository , IMapper mapper) : CreateCommandHandler<CreateStockCommand, Inventory.Domain.Stock>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
