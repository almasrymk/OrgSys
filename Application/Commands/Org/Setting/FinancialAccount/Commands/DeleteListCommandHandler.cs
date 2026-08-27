namespace Application.Commands.Org.Setting.FinancialAccount.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Shared;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialAccountCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FinancialAccount> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListFinancialAccountCommand, Domain.Entities.FinancialAccount>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.FinancialAccount, bool>> CreateFilter(DeleteListFinancialAccountCommand request) =>
            e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;

        public override async Task<bool> RemoveDetails(DeleteListFinancialAccountCommand request)
        {
            await RemoveDetails<CashBox>(e => e.FinancialAccountId != null && request.Ids.Contains(e.FinancialAccountId.Value));
            await RemoveDetails<BankAccount>(e => e.FinancialAccountId != null && request.Ids.Contains(e.FinancialAccountId.Value));
            return true;
        }
    }
}
