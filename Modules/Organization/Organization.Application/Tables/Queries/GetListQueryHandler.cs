namespace Organization.Application.Tables.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListTableQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<TableDto> , IListQuery<ResultCollection<TableDto>>;

    public sealed class GetListQueryHandler(IRepository<Organization.Domain.Table> _Repository, IMapper mapper) : ListCommandHandler<GetListTableQuery, Organization.Domain.Table, TableDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Table, bool>> CreateFilter(GetListTableQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Organization.Domain.Table>, IOrderedQueryable<Organization.Domain.Table>> CreateOrderBy(GetListTableQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}