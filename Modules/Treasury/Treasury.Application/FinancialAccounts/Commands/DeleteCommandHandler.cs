namespace Treasury.Application.FinancialAccounts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteFinancialAccountCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.FinancialAccount> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteFinancialAccountCommand, Treasury.Domain.FinancialAccount>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(DeleteFinancialAccountCommand request) =>
            e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;

        public override async Task<bool> RemoveDetails(DeleteFinancialAccountCommand request)
        {
            await RemoveDetails<CashBox>(e => e.FinancialAccountId == request.Id);
            await RemoveDetails<BankAccount>(e => e.FinancialAccountId == request.Id);
            return true;
        }
    }
}
