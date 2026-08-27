namespace Application.Commands.Org.Setting.FinancialAccount.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Shared;
    using System.Linq.Expressions;

    public sealed record DeleteFinancialAccountCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FinancialAccount> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteFinancialAccountCommand, Domain.Entities.FinancialAccount>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.FinancialAccount, bool>> CreateFilter(DeleteFinancialAccountCommand request) =>
            e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;

        public override async Task<bool> RemoveDetails(DeleteFinancialAccountCommand request)
        {
            await RemoveDetails<CashBox>(e => e.FinancialAccountId == request.Id);
            await RemoveDetails<BankAccount>(e => e.FinancialAccountId == request.Id);
            return true;
        }
    }
}
