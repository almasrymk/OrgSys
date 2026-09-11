namespace Accounting.Application.Accounts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListAccountCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.Account> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListAccountCommand, Accounting.Domain.Account>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Accounting.Domain.Account, bool>> CreateFilter(DeleteListAccountCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}