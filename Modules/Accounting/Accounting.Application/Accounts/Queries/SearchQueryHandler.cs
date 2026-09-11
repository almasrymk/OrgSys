namespace Accounting.Application.Accounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;
    
    public sealed record SearchAccountQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<AccountDto> ,ISearchQuery<ResultPagination<AccountDto>>;

    public sealed class SearchQueryHandler(IRepository<Accounting.Domain.Account> _Repository, IMapper mapper) : SearchCommandHandler<SearchAccountQuery, Accounting.Domain.Account, AccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.Account, bool>> CreateFilter(SearchAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Accounting.Domain.Account>, IOrderedQueryable<Accounting.Domain.Account>> CreateOrderBy(SearchAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}