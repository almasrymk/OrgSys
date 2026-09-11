namespace Inventory.Application.Products.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchProductQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductDto> ,ISearchQuery<ResultPagination<ProductDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Product> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductQuery, Inventory.Domain.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Product, bool>> CreateFilter(SearchProductQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Inventory.Domain.Product>, IOrderedQueryable<Inventory.Domain.Product>> CreateOrderBy(SearchProductQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Classification,Dealer,ProductUnits,ProductRecipes,ProductPropertyElements";
        }
    }
}