namespace Treasury.Application.FinancialAccounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListFinancialAccountQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FinancialAccountDto>, IListQuery<ResultCollection<FinancialAccountDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.FinancialAccount> _Repository, IMapper mapper) : ListCommandHandler<GetListFinancialAccountQuery, Treasury.Domain.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(GetListFinancialAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (request.TypeId == 0 || (long)e.FinancialAccountType == request.TypeId) &&
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch) || (e.Code != null && e.Code.Contains(request.KeySearch))) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "CashBox,BankAccount.Bank,BankAccount.BankBranch,Account,Currency";
        }

        override public Func<IQueryable<Treasury.Domain.FinancialAccount>, IOrderedQueryable<Treasury.Domain.FinancialAccount>> CreateOrderBy(GetListFinancialAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
