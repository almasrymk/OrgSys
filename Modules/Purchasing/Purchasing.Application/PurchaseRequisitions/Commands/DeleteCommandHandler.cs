namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeletePurchaseRequisitionCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseRequisition> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeletePurchaseRequisitionCommand, PurchaseRequisition>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<PurchaseRequisition, bool>> CreateFilter(DeletePurchaseRequisitionCommand request)
        {
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}
