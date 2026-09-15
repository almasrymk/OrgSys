namespace Catalog.Application.Products.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListProductQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductDto> , IListQuery<ResultCollection<ProductDto>>;

    public sealed class GetListQueryHandler(IRepository<Catalog.Domain.Product> _Repository, IMapper mapper) : ListCommandHandler<GetListProductQuery, Catalog.Domain.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Product, bool>> CreateFilter(GetListProductQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Catalog.Domain.Product>, IOrderedQueryable<Catalog.Domain.Product>> CreateOrderBy(GetListProductQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Classification,Dealer,Brand,ProductUnits,ProductPropertyElements";
        }
    }
}