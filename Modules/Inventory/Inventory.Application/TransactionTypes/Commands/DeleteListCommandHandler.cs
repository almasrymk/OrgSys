namespace Inventory.Application.TransactionTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListTransactionTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.TransactionType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTransactionTypeCommand, Inventory.Domain.TransactionType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.TransactionType, bool>> CreateFilter(DeleteListTransactionTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}