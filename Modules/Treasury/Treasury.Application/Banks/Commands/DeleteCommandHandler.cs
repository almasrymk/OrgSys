namespace Treasury.Application.Banks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteBankCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Bank> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBankCommand, Treasury.Domain.Bank>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Treasury.Domain.Bank, bool>> CreateFilter(DeleteBankCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}