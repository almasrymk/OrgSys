namespace Inventory.Application.Stocks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteStockCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Stock> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteStockCommand, Inventory.Domain.Stock>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Stock, bool>> CreateFilter(DeleteStockCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}