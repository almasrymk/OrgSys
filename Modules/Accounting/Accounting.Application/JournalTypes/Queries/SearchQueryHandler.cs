namespace Accounting.Application.JournalTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchJournalTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<JournalTypeDto>, ISearchQuery<ResultPagination<JournalTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Accounting.Domain.JournalType> repository, IMapper mapper)
        : SearchCommandHandler<SearchJournalTypeQuery, Accounting.Domain.JournalType, JournalTypeDto>(repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.JournalType, bool>> CreateFilter(SearchJournalTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            return e =>
                (string.IsNullOrEmpty(request.KeySearch) || (e.Name ?? "").Contains(request.KeySearch)) &&
                e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<Accounting.Domain.JournalType>, IOrderedQueryable<Accounting.Domain.JournalType>> CreateOrderBy(SearchJournalTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}
