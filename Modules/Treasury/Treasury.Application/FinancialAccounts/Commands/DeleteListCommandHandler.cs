namespace Treasury.Application.FinancialAccounts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialAccountCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.FinancialAccount> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListFinancialAccountCommand, Treasury.Domain.FinancialAccount>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(DeleteListFinancialAccountCommand request) =>
            e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;

        public override async Task<bool> RemoveDetails(DeleteListFinancialAccountCommand request)
        {
            await RemoveDetails<CashBox>(e => e.FinancialAccountId != null && request.Ids.Contains(e.FinancialAccountId.Value));
            await RemoveDetails<BankAccount>(e => e.FinancialAccountId != null && request.Ids.Contains(e.FinancialAccountId.Value));
            return true;
        }
    }
}
