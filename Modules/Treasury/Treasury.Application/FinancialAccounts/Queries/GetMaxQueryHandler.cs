namespace Treasury.Application.FinancialAccounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record GetMaxFinancialAccountQuery(long TypeId, long ParentId) : ICommandOb<object>, IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Treasury.Domain.FinancialAccount> _Repository) : GetMaxCommandHandler<GetMaxFinancialAccountQuery, Treasury.Domain.FinancialAccount>(_Repository)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(GetMaxFinancialAccountQuery request)
        {
            return e => (long)e.TypeId == request.TypeId && e.Status != OrgSys.SharedKernel.Status.Deleted;
        }

        public override Expression<Func<Treasury.Domain.FinancialAccount, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
