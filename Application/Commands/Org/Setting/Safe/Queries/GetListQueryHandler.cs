namespace Application.Commands.Org.Setting.Safe.Queries
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

    public sealed record GetListSafeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<SafeModelView> , IListQuery<ResultCollection<SafeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Safe> _Repository, IMapper mapper) : ListCommandHandler<GetListSafeQuery, Entity.Model.Safe, SafeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Safe, bool>> CreateFilter(GetListSafeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Safe>, IOrderedQueryable<Entity.Model.Safe>> CreateOrderBy(GetListSafeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}