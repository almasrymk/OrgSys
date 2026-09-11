namespace Treasury.Application.BankBranches.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListBankBranchCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.BankBranch> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListBankBranchCommand, Treasury.Domain.BankBranch>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(DeleteListBankBranchCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}