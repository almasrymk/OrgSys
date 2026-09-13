namespace Administration.Application.Preferences.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    
    using System.Linq.Expressions;

    public sealed record GetListPreferenceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PreferenceDto> , IListQuery<ResultCollection<PreferenceDto>>;

    public sealed class GetListQueryHandler(IRepository<Administration.Domain.Preference> _Repository, IMapper mapper) : ListCommandHandler<GetListPreferenceQuery, Administration.Domain.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Administration.Domain.Preference, bool>> CreateFilter(GetListPreferenceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
             (string.IsNullOrEmpty(request.KeySearch) || e.Reference.Contains(request.KeySearch)) &&
            (request.ParentId == 0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Administration.Domain.Preference>, IOrderedQueryable<Administration.Domain.Preference>> CreateOrderBy(GetListPreferenceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}