namespace Treasury.Application.FinancialAccounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchFinancialAccountQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<FinancialAccountDto>, ISearchQuery<ResultPagination<FinancialAccountDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.FinancialAccount> _Repository, IMapper mapper) : SearchCommandHandler<SearchFinancialAccountQuery, Treasury.Domain.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.FinancialAccount, bool>> CreateFilter(SearchFinancialAccountQuery request)
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

        override public Func<IQueryable<Treasury.Domain.FinancialAccount>, IOrderedQueryable<Treasury.Domain.FinancialAccount>> CreateOrderBy(SearchFinancialAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
