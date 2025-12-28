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

    public sealed record GetListProductQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductModelView> , IListQuery<ResultCollection<ProductModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Product> _Repository, IMapper mapper) : ListCommandHandler<GetListProductQuery, Entity.Model.Product, ProductModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Product, bool>> CreateFilter(GetListProductQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Product>, IOrderedQueryable<Entity.Model.Product>> CreateOrderBy(GetListProductQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}