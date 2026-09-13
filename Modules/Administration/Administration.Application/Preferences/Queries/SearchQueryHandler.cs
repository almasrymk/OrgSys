namespace Administration.Application.Preferences.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    
    using System.Linq.Expressions;

    public sealed record SearchPreferenceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PreferenceDto> ,ISearchQuery<ResultPagination<PreferenceDto>>;

    public sealed class SearchQueryHandler(IRepository<Administration.Domain.Preference> _Repository, IMapper mapper) : SearchCommandHandler<SearchPreferenceQuery, Administration.Domain.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Administration.Domain.Preference, bool>> CreateFilter(SearchPreferenceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
             (string.IsNullOrEmpty(request.KeySearch) || e.Reference.Contains(request.KeySearch)) &&
            (request.ParentId == 0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Administration.Domain.Preference>, IOrderedQueryable<Administration.Domain.Preference>> CreateOrderBy(SearchPreferenceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}