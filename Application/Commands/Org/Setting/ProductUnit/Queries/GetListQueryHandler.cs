using Application.Abstraction.Command;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Application.DTOs;

namespace Application.Commands.Org.Setting.ProductUnit.Queries
{
    public sealed record GetListProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductUnitModelView> , IListQuery<ResultCollection<ProductUnitModelView>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.ProductUnit> _Repository, IMapper mapper) : ListCommandHandler<GetListProductUnitQuery, Domain.Entities.ProductUnit, ProductUnitModelView>(_Repository, mapper)
    {       
        override public Func<IQueryable<Domain.Entities.ProductUnit>, IOrderedQueryable<Domain.Entities.ProductUnit>> CreateOrderBy(GetListProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}