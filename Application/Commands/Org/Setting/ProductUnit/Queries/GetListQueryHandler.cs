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
    public sealed record GetListProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductUnitModelView> , IListQuery<ResultCollection<ProductUnitModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.ProductUnit> _Repository, IMapper mapper) : ListCommandHandler<GetListProductUnitQuery, Entity.Model.ProductUnit, ProductUnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.ProductUnit, bool>> CreateFilter(GetListProductUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => e.Status != Status.Deleted && e.Hide != true;
        }
        
        public override Func<IQueryable<Entity.Model.ProductUnit>, IOrderedQueryable<Entity.Model.ProductUnit>> CreateOrderBy(GetListProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}