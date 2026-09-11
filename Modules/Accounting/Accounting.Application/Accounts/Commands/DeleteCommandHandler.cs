namespace Accounting.Application.Accounts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteAccountCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.Account> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteAccountCommand, Accounting.Domain.Account>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Accounting.Domain.Account, bool>> CreateFilter(DeleteAccountCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}