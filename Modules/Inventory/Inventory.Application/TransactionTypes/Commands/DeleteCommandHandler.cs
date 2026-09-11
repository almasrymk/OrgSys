namespace Inventory.Application.TransactionTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteTransactionTypeCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.TransactionType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTransactionTypeCommand, Inventory.Domain.TransactionType>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Inventory.Domain.TransactionType, bool>> CreateFilter(DeleteTransactionTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}