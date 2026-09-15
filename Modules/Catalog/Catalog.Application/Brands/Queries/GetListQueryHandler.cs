namespace Catalog.Application.Brands.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListBrandQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<BrandDto>, IListQuery<ResultCollection<BrandDto>>;

    public sealed class GetListQueryHandler(IRepository<Catalog.Domain.Brand> _Repository, IMapper mapper) : ListCommandHandler<GetListBrandQuery, Catalog.Domain.Brand, BrandDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Brand, bool>> CreateFilter(GetListBrandQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Catalog.Domain.Brand>, IOrderedQueryable<Catalog.Domain.Brand>> CreateOrderBy(GetListBrandQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
