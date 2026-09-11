namespace MasterData.Application.ReferenceTypes.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchReferenceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ReferenceTypeDto> ,ISearchQuery<ResultPagination<ReferenceTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<MasterData.Domain.ReferenceType> _Repository, IMapper mapper) : SearchCommandHandler<SearchReferenceTypeQuery, MasterData.Domain.ReferenceType, ReferenceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.ReferenceType, bool>> CreateFilter(SearchReferenceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<MasterData.Domain.ReferenceType>, IOrderedQueryable<MasterData.Domain.ReferenceType>> CreateOrderBy(SearchReferenceTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}
