namespace Purchasing.Application.PurchaseOrders.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListPurchaseOrderCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseOrder> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListPurchaseOrderCommand, PurchaseOrder>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<PurchaseOrder, bool>> CreateFilter(DeleteListPurchaseOrderCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}
