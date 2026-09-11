namespace Accounting.Application.Journals.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchJournalQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<JournalDto> ,ISearchQuery<ResultPagination<JournalDto>>;

    public sealed class SearchQueryHandler(IRepository<Accounting.Domain.Journal> _Repository, IMapper mapper) : SearchCommandHandler<SearchJournalQuery, Accounting.Domain.Journal, JournalDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.Journal, bool>> CreateFilter(SearchJournalQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Currency,JournalType,FiscalYear,OriginalJournal,ReversalJournal";
        }

        override public Func<IQueryable<Accounting.Domain.Journal>, IOrderedQueryable<Accounting.Domain.Journal>> CreateOrderBy(SearchJournalQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
