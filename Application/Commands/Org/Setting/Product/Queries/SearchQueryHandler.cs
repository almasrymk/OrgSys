namespace Application.Commands.Org.Setting.Product.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchProductQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductModelView> ,ISearchQuery<ResultPagination<ProductModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Product> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductQuery, Entity.Model.Product, ProductModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Product, bool>> CreateFilter(SearchProductQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Product>, IOrderedQueryable<Entity.Model.Product>> CreateOrderBy(SearchProductQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Classification,Dealer,ProductUnits,ProductUnits.Unit,ProductRecipes,ProductPropertyElements";
        }
    }
}