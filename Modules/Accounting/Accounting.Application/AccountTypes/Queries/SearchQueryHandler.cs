namespace Accounting.Application.AccountTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchAccountTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<AccountTypeDto> ,ISearchQuery<ResultPagination<AccountTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Accounting.Domain.AccountType> _Repository, IMapper mapper) : SearchCommandHandler<SearchAccountTypeQuery, Accounting.Domain.AccountType, AccountTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.AccountType, bool>> CreateFilter(SearchAccountTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Accounting.Domain.AccountType>, IOrderedQueryable<Accounting.Domain.AccountType>> CreateOrderBy(SearchAccountTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}