namespace Treasury.Application.Banks.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchBankQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BankDto> ,ISearchQuery<ResultPagination<BankDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.Bank> _Repository, IMapper mapper) : SearchCommandHandler<SearchBankQuery, Treasury.Domain.Bank, BankDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Bank, bool>> CreateFilter(SearchBankQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Treasury.Domain.Bank>, IOrderedQueryable<Treasury.Domain.Bank>> CreateOrderBy(SearchBankQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}