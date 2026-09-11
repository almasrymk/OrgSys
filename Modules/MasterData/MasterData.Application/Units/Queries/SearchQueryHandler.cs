namespace MasterData.Application.Units.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<UnitDto> ,ISearchQuery<ResultPagination<UnitDto>>;

    public sealed class SearchQueryHandler(IRepository<MasterData.Domain.Unit> _Repository, IMapper mapper) : SearchCommandHandler<SearchUnitQuery, MasterData.Domain.Unit, UnitDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Unit, bool>> CreateFilter(SearchUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.Unit>, IOrderedQueryable<MasterData.Domain.Unit>> CreateOrderBy(SearchUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
