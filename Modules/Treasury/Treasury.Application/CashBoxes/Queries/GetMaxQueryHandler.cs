namespace Treasury.Application.CashBoxes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record GetMaxCashBoxQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Treasury.Domain.CashBox> _Repository) : GetMaxCommandHandler<GetMaxCashBoxQuery, Treasury.Domain.CashBox>(_Repository)
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
