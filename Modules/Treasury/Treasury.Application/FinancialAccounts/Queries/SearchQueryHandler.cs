namespace Treasury.Application.FinancialAccounts.Queries
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchFinancialAccountQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<FinancialAccountDto>, ISearchQuery<ResultPagination<FinancialAccountDto>>;

    public sealed class SearchQueryHandler(
        IRepository<Treasury.Domain.FinancialAccount> _Repository,
        ISender sender,
        IMapper mapper) : SearchCommandHandler<SearchFinancialAccountQuery, Treasury.Domain.FinancialAccount, FinancialAccountDto>(_Repository, mapper)
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

        // Account was removed from this list (see the GeneralLedger migration report) —
        // AccountCode/AccountName are patched in below instead.
        public override string CreateInclude()
        {
            return "CashBox,BankAccount.Bank,BankAccount.BankBranch,Currency";
        }

        override public Func<IQueryable<Treasury.Domain.FinancialAccount>, IOrderedQueryable<Treasury.Domain.FinancialAccount>> CreateOrderBy(SearchFinancialAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override async Task<ResultPagination<FinancialAccountDto>> Handle(SearchFinancialAccountQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            var accountIds = result.Response.Where(e => e.AccountId is > 0).Select(e => e.AccountId!.Value).Distinct().ToList();
            if (accountIds.Count > 0)
            {
                var accounts = (await sender.Send(new GetAccountLookupsQuery(accountIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.AccountId is > 0 && accounts.TryGetValue(dto.AccountId.Value, out var account))
                    {
                        dto.AccountCode = account.Code;
                        dto.AccountName = account.Name;
                    }
            }

            return result;
        }
    }
}
