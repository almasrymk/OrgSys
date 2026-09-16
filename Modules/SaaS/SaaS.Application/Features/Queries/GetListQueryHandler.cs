namespace SaaS.Application.Features.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListFeatureQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FeatureDto>, IListQuery<ResultCollection<FeatureDto>>;

    public sealed class GetListQueryHandler(IRepository<Feature> _Repository, IMapper mapper) : ListCommandHandler<GetListFeatureQuery, Feature, FeatureDto>(_Repository, mapper)
    {
        public override Expression<Func<Feature, bool>> CreateFilter(GetListFeatureQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch) || e.Key!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Feature>, IOrderedQueryable<Feature>> CreateOrderBy(GetListFeatureQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
