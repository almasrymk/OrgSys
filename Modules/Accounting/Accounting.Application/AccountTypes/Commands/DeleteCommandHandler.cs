namespace Accounting.Application.AccountTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteAccountTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.AccountType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteAccountTypeCommand, Accounting.Domain.AccountType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Accounting.Domain.AccountType, bool>> CreateFilter(DeleteAccountTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}