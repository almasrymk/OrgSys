namespace Application.Commands.Org.Setting.Safe.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListSafeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<SafeModelView> , IListQuery<ResultCollection<SafeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Safe> _Repository, IMapper mapper) : ListCommandHandler<GetListSafeQuery, Domain.Entities.Safe, SafeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Safe, bool>> CreateFilter(GetListSafeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Safe>, IOrderedQueryable<Domain.Entities.Safe>> CreateOrderBy(GetListSafeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}