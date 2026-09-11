namespace Application.Commands.Org.Setting.Preference.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Domain.Abstraction;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchPreferenceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PreferenceDto> ,ISearchQuery<ResultPagination<PreferenceDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Preference> _Repository, IMapper mapper) : SearchCommandHandler<SearchPreferenceQuery, Domain.Entities.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(SearchPreferenceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
             (string.IsNullOrEmpty(request.KeySearch) || e.Reference.Contains(request.KeySearch)) &&
            (request.ParentId == 0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Preference>, IOrderedQueryable<Domain.Entities.Preference>> CreateOrderBy(SearchPreferenceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}