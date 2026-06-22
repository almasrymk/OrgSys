using Application.Abstraction.Command;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Application.DTOs;

namespace Application.Commands.Org.Setting.ProductUnit.Queries
{
    public sealed record SearchProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductUnitModelView> ,ISearchQuery<ResultPagination<ProductUnitModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.ProductUnit> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductUnitQuery, Domain.Entities.ProductUnit, ProductUnitModelView>(_Repository, mapper)
    {        
        override public Func<IQueryable<Domain.Entities.ProductUnit>, IOrderedQueryable<Domain.Entities.ProductUnit>> CreateOrderBy(SearchProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}