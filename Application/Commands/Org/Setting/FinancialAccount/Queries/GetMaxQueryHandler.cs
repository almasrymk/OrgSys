namespace Application.Commands.Org.Setting.FinancialAccount.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using System.Linq.Expressions;

    public sealed record GetMaxFinancialAccountQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Domain.Entities.FinancialAccount> _Repository) : GetMaxCommandHandler<GetMaxFinancialAccountQuery, Domain.Entities.FinancialAccount>(_Repository)
    {
        public override Expression<Func<Domain.Entities.FinancialAccount, bool>> CreateFilter(GetMaxFinancialAccountQuery request)
        {
            return e => (long)e.TypeId == request.TypeId && e.Status != Domain.Enums.Status.Deleted;
        }

        public override Expression<Func<Domain.Entities.FinancialAccount, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
