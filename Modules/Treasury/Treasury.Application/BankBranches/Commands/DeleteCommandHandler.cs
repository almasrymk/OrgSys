namespace Treasury.Application.BankBranches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteBankBranchCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.BankBranch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBankBranchCommand, Treasury.Domain.BankBranch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(DeleteBankBranchCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}