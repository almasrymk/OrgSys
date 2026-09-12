namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListPurchaseRequisitionCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseRequisition> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListPurchaseRequisitionCommand, PurchaseRequisition>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<PurchaseRequisition, bool>> CreateFilter(DeleteListPurchaseRequisitionCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}
