namespace Application.Commands.Org.Setting.Table.Queries
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

    public sealed record GetListTableQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<TableModelView> , IListQuery<ResultCollection<TableModelView>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Table> _Repository, IMapper mapper) : ListCommandHandler<GetListTableQuery, Domain.Entities.Table, TableModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Table, bool>> CreateFilter(GetListTableQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Table>, IOrderedQueryable<Domain.Entities.Table>> CreateOrderBy(GetListTableQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}