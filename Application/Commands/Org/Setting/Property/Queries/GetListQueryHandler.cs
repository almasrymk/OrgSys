namespace Application.Commands.Org.Setting.Property.Queries
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

    public sealed record GetListPropertyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PropertyModelView> , IListQuery<ResultCollection<PropertyModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Property> _Repository, IMapper mapper) : ListCommandHandler<GetListPropertyQuery, Entity.Model.Property, PropertyModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Property, bool>> CreateFilter(GetListPropertyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Property>, IOrderedQueryable<Entity.Model.Property>> CreateOrderBy(GetListPropertyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}