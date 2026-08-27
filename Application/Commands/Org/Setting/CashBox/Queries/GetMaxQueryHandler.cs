namespace Application.Commands.Org.Setting.CashBox.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using System.Linq.Expressions;

    public sealed record GetMaxCashBoxQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Domain.Entities.CashBox> _Repository) : GetMaxCommandHandler<GetMaxCashBoxQuery, Domain.Entities.CashBox>(_Repository)
    {
        public override Expression<Func<CashBox, bool>> CreateFilter(GetMaxCashBoxQuery request)
        {
            return e => e.TypeId == request.TypeId;
        }

        public override Expression<Func<CashBox, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
