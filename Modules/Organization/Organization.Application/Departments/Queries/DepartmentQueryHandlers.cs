namespace Organization.Application.Departments.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdDepartmentQuery(long Id) : ICommand<DepartmentDto>, IGetByIdQuery<Result<DepartmentDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Organization.Domain.Department> _Repository, IMapper mapper)
        : GetCommandHandler<GetByIdDepartmentQuery, Organization.Domain.Department, DepartmentDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Department, bool>> CreateFilter(GetByIdDepartmentQuery request) =>
            e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
    }

    public sealed record GetListDepartmentQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<DepartmentDto>, IListQuery<ResultCollection<DepartmentDto>>;

    public sealed class GetListQueryHandler(IRepository<Organization.Domain.Department> _Repository, IMapper mapper)
        : ListCommandHandler<GetListDepartmentQuery, Organization.Domain.Department, DepartmentDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Department, bool>> CreateFilter(GetListDepartmentQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            return e =>
                (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
                e.Status != Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<Organization.Domain.Department>, IOrderedQueryable<Organization.Domain.Department>> CreateOrderBy(GetListDepartmentQuery request) =>
            q => q.OrderByDescending(e => e.Id);
    }

    public sealed record SearchDepartmentQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<DepartmentDto>, ISearchQuery<ResultPagination<DepartmentDto>>;

    public sealed class SearchQueryHandler(IRepository<Organization.Domain.Department> _Repository, IMapper mapper)
        : SearchCommandHandler<SearchDepartmentQuery, Organization.Domain.Department, DepartmentDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Department, bool>> CreateFilter(SearchDepartmentQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            return e =>
                (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
                e.Status != Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<Organization.Domain.Department>, IOrderedQueryable<Organization.Domain.Department>> CreateOrderBy(SearchDepartmentQuery request) =>
            q => q.OrderByDescending(e => e.Id);
    }
}
