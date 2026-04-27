using System.Linq.Expressions;
using Application.Abstraction.Command;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Entity.ModelView;
using Utility;

namespace Application.Commands.Org.Setting.ProductUnit.Queries
{
    public sealed record SearchProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductUnitModelView> ,ISearchQuery<ResultPagination<ProductUnitModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.ProductUnit> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductUnitQuery, Entity.Model.ProductUnit, ProductUnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.ProductUnit, bool>> CreateFilter(SearchProductUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.ProductUnit>, IOrderedQueryable<Entity.Model.ProductUnit>> CreateOrderBy(SearchProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}