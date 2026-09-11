namespace Organization.Application.Shifts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchShiftQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ShiftDto> ,ISearchQuery<ResultPagination<ShiftDto>>;

    public sealed class SearchQueryHandler(IRepository<Organization.Domain.Shift> _Repository, IMapper mapper) : SearchCommandHandler<SearchShiftQuery, Organization.Domain.Shift, ShiftDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Shift, bool>> CreateFilter(SearchShiftQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Organization.Domain.Shift>, IOrderedQueryable<Organization.Domain.Shift>> CreateOrderBy(SearchShiftQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}