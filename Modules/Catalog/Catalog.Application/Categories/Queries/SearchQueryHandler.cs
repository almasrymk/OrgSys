namespace Catalog.Application.Categories.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchClassificationQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ClassificationDto> ,ISearchQuery<ResultPagination<ClassificationDto>>;

    public sealed class SearchQueryHandler(IRepository<Catalog.Domain.Classification> _Repository, IMapper mapper) : SearchCommandHandler<SearchClassificationQuery, Catalog.Domain.Classification, ClassificationDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Classification, bool>> CreateFilter(SearchClassificationQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Catalog.Domain.Classification>, IOrderedQueryable<Catalog.Domain.Classification>> CreateOrderBy(SearchClassificationQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
