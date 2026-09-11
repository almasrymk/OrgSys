namespace Accounting.Application.AccountTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListAccountTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.AccountType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListAccountTypeCommand, Accounting.Domain.AccountType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Accounting.Domain.AccountType, bool>> CreateFilter(DeleteListAccountTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}