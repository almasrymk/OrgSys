namespace Treasury.Application.FinancialTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchFinancialTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) 
        : ICommandPagination<FinancialTypeDto> ,ISearchQuery<ResultPagination<FinancialTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.FinancialType> _Repository, IMapper mapper) : 
        SearchCommandHandler<SearchFinancialTypeQuery, Treasury.Domain.FinancialType, FinancialTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.FinancialType, bool>> CreateFilter(SearchFinancialTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || ("" + e.Code).Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "";
        }

        override public Func<IQueryable<Treasury.Domain.FinancialType>, IOrderedQueryable<Treasury.Domain.FinancialType>> CreateOrderBy(SearchFinancialTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}