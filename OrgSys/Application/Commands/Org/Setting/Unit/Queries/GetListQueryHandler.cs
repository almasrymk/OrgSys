namespace Application.Commands.Org.Setting.Unit.Queries
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

    public sealed record GetListUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<UnitModelView> , IListQuery<ResultCollection<UnitModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Unit> _Repository, IMapper mapper) : ListCommandHandler<GetListUnitQuery, Entity.Model.Unit, UnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Unit, bool>> CreateFilter(GetListUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Unit>, IOrderedQueryable<Entity.Model.Unit>> CreateOrderBy(GetListUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}