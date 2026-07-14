namespace Application.Commands.Org.Setting.Product.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchProductQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductDto> ,ISearchQuery<ResultPagination<ProductDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Product> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductQuery, Domain.Entities.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Product, bool>> CreateFilter(SearchProductQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Product>, IOrderedQueryable<Domain.Entities.Product>> CreateOrderBy(SearchProductQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Classification,Dealer,ProductUnits,ProductRecipes,ProductPropertyElements";
        }
    }
}