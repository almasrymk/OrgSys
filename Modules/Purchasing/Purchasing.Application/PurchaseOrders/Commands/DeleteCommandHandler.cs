namespace Purchasing.Application.PurchaseOrders.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeletePurchaseOrderCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseOrder> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeletePurchaseOrderCommand, PurchaseOrder>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<PurchaseOrder, bool>> CreateFilter(DeletePurchaseOrderCommand request)
        {
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}
