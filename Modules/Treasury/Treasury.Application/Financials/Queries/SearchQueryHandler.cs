namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<FinancialDto> ,ISearchQuery<ResultPagination<FinancialDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper) : SearchCommandHandler<SearchFinancialQuery, Treasury.Domain.Financial, FinancialDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(SearchFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) ||
             (e.Code != null && e.Code.Contains(request.KeySearch)) ||
             (e.Notes != null && e.Notes.Contains(request.KeySearch)) ||
             (e.ReferenceNumber != null && e.ReferenceNumber.Contains(request.KeySearch)) ||
             (e.FinancialAccount != null && e.FinancialAccount.Name.Contains(request.KeySearch))) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.FinancialTypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Currency,FinancialAccount,ContraFinancialAccount,FinancialType,Journal";
        }

        override public Func<IQueryable<Treasury.Domain.Financial>, IOrderedQueryable<Treasury.Domain.Financial>> CreateOrderBy(SearchFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
