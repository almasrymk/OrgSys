namespace Inventory.Application.Stocks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListStockCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Stock> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListStockCommand, Inventory.Domain.Stock>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(DeleteListStockCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}