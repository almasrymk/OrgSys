namespace SaaS.Application.Features.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchFeatureQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<FeatureDto>, ISearchQuery<ResultPagination<FeatureDto>>;

    public sealed class SearchQueryHandler(IRepository<Feature> _Repository, IMapper mapper) : SearchCommandHandler<SearchFeatureQuery, Feature, FeatureDto>(_Repository, mapper)
    {
        public override Expression<Func<Feature, bool>> CreateFilter(SearchFeatureQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch) || e.Key!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Feature>, IOrderedQueryable<Feature>> CreateOrderBy(SearchFeatureQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
